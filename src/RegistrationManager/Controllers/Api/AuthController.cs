using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using RegistrationManager.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Swashbuckle.SwaggerGen.Annotations;

namespace RegistrationManager.Controllers.Api
{
    [Route("auth/")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private ILogger<AuthController> logger;
        private IRegistrationsRepository repository;
        private SignInManager<DbUserIdentity> signInMgt;

        public AuthController(DbManagerContext dbMangerInjection,
            ILogger<AuthController> loggerFactory,
            IRegistrationsRepository registrationRepository,
            SignInManager<DbUserIdentity> signInManager)
        {
            repository = registrationRepository;
            logger = loggerFactory;
            signInMgt = signInManager;
        }

        /// <summary>
        /// Check whether the user already exist in the database and if one will be login, 
        /// in order to user the service. <paramref name="login"/>
        /// </summary>
        /// <param name="login">
        /// The login information about the user; 
        /// which include the email and password.
        /// </param>
        /// <returns> 
        /// Task is a asyncronous param which binds the IActionResult. 
        /// IActionResult represent the current state of the service.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Login), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Login), 507)]//TODO create enum in order to catch the remaining http status codes. 
        public async Task<IActionResult> LoginPost([FromBody, Required] Login login)
        {
            if (ModelState.IsValid)
            {   
                var result = await signInMgt.PasswordSignInAsync(login.Email, login.Password, true, false);
                
                if (result.Succeeded)
                {
                    logger.LogInformation($"User had been added to the database. (200)");
                    return Ok("User had been added to the database.");
                } 
                logger.LogError($"DB request failed (507)");
                return StatusCode(507, login);
            }
            logger.LogError($"Internal server error (400)");
            return BadRequest($"Probably the credentials are not valid. {login}");
        }

        /// <summary>
        /// Registration service in order to add a new user to the DB. 
        /// If the registration failed a appropriate html status code will be returned. 
        /// </summary>
        /// <param name="registration">
        /// Registration informatio which expands the login credentials via a password confirmation.
        /// This is required in order to check the correctnes of the password.
        /// </param>
        /// <returns> 
        /// Task is a asyncronous param which binds the IActionResult. 
        /// IActionResult represent the current state of the service.
        /// In case of an exception an interal server error will be returned. 
        /// </returns>
        [HttpPost("registration")]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.MultipleChoices)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(Register), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegistrationPost([FromBody, Required] Register registration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //TODO There must be a better in oder to check whether a user is sign in or not.
                    var result = await signInMgt.PasswordSignInAsync(registration.Email, registration.Password, true, false);

                    if (!result.Succeeded)
                    {
                        if (repository.CreateEntry(registration).Result)
                        {
                            logger.LogInformation("A new user had been added to the database");
                            return StatusCode(201);
                        }
                        logger.LogTrace("User could not be added to the database.");
                        return StatusCode(403, "User could not be added to the database.");
                    }
                    logger.LogWarning("User is already stored in the database.");
                    return StatusCode(300, "User is already stored in the database.");
                }
                catch (Exception ex)
                {
                    logger.LogError($"DB request failed. {ex.Message}");
                    return StatusCode(500, "Internal server error, most likely because of the DB.");
                }
            }
            logger.LogInformation("User cannot be stored in the DB, because the credentials are most likely not valid.");
            return BadRequest($"Probably the credentials are not valid. {registration}");
        }       
    }
}
