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
    /// <summary>
    /// The class manage he authorization and authentication of an user.
    /// All responsed and response will be done by means of json format.
    /// Moreover, the descriton of the resources are also being used by the Swashbuckle (Swagger) framework. 
    /// </summary>
    [Route("auth/")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private ILogger<AuthController> logger;
        private IRegistrationsRepository repository;
        private SignInManager<DbUserIdentity> signInMgt;

        /// <summary>
        /// The authorization and authentication controller.
        /// Check whether a certain user is registed in the database. 
        /// If not the service provides the ressource in order to store the user in the database.
        /// Appropriate html status codes will be returned. 
        /// </summary>
        /// <param name="loggerFactory">
        /// Logs all steps in case of an error.
        /// </param>
        /// <param name="registrationRepository">
        /// Repository pattern which handles all database call.
        /// </param>
        /// <param name="signInManager">
        /// Sign in manager stores and check the user credentials by means of the database.
        /// </param>
        public AuthController(ILogger<AuthController> loggerFactory,
            IRegistrationsRepository registrationRepository,
            SignInManager<DbUserIdentity> signInManager)
        {
            repository = registrationRepository;
            logger = loggerFactory;
            signInMgt = signInManager;
        }

        /// <summary>
        /// Check whether the user already exist in the database. 
        /// This is required to use further services. 
        /// </summary>
        /// <param name="login">
        /// Login informatio about the user, which need to be comform.
        /// </param>
        /// <returns> 
        /// Task is a asyncronous param which binds the IActionResult. 
        /// IActionResult represent the current state of the service.
        /// </returns>
        /// <response code="200">User had been added to the database.</response>
        /// <response code="507">Database request failed</response>
        /// <response code="400">Probably the credentials are not valid.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Microsoft.AspNetCore.Identity.SignInResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), 507)]//TODO create enum in order to catch the remaining http status codes. 
        [ProducesResponseType(typeof(Login), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LoginPost([FromBody, Required] Login login)
        {
            if (ModelState.IsValid)
            {   
                //TODO Does it store the current state of the user? because that should not happen?
                var result = await signInMgt.PasswordSignInAsync(login.Email, login.Password, true, false);
                
                if (result.Succeeded)
                {
                    logger.LogInformation("User had been added to the database. (200)");
                    return Ok(result); 
                } 
                logger.LogError("Database request failed (507)");
                return StatusCode(507, login);
            }
            logger.LogError("Internal server error (400)");
            return BadRequest("Probably the credentials are not valid.");
        }

        /// <summary>
        /// Registration service adds a new user to the database. 
        /// If the registration failed a appropriate html status code will be returned.
        /// Keep in mind the username and email are the same, currently they are not seperated.
        /// Therefore, please use the email attribute only!
        /// </summary>
        /// <param name="registration">
        /// Registration informatio about the user.
        /// </param>
        /// <returns> 
        /// Task is a asyncronous param which binds the IActionResult. 
        /// IActionResult represent the current state of the service.
        /// In case of an exception an interal server error will be returned. 
        /// </returns>
        /// <response code="201">A new user had been added to the database.</response>
        /// <response code="403">User could not be added to the database</response>
        /// <response code="300">User is already stored in the database.</response>
        /// <response code="500">Internal server error, most likely because of the database.</response>
        /// <response code="400">Probably the credentials are not valid.</response>
        //TODO Commends are the same as the logging information need to be collected together.
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
                            logger.LogInformation("A new user had been added to the database.");
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
                    return StatusCode(500, "Internal server error, most likely because of the database.");
                }
            }
            logger.LogInformation("User cannot be stored in the DB, because the credentials are most likely not valid.(400)");
            return BadRequest($"Probably the credentials are not valid. {registration}");
        }      
    }
}
