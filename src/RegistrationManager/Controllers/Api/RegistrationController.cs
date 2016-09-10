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

namespace RegistrationManager.Controllers.Api
{
    [Route("api/")]
    public class RegistrationController : Controller
    {
        //TODO Using the logger
        private ILogger<RegistrationController> logger;
        private IRegistrationsRepository repository;
        private SignInManager<DbUserIdentity> signInMgt;

        public RegistrationController(DbManagerContext dbMangerInjection, 
            ILogger<RegistrationController> loggerFactory, 
            IRegistrationsRepository registrationRepository,
            SignInManager<DbUserIdentity> signInManager)
        {
            //DB queries will be implemented in the repository call (pattern).
            repository = registrationRepository;
            logger = loggerFactory;
            signInMgt = signInManager;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> LoginPost([FromBody] Credential credentials)
        {
            if (ModelState.IsValid)
            {   
                var result = await signInMgt.PasswordSignInAsync(credentials.Email, credentials.Password, true, false);

                if (result.Succeeded)
                {
                    //200
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect");
                    logger.LogError($"DB request failed, maybe because email and/or password is incorrect?");
                    //TODO Implement the error redirection 
                    return Redirect("/error");
                }
            }
            //400
            return BadRequest();
        }

        [HttpPost("registration")]
        public IActionResult RegistrationPost([FromBody] Credential credentials)
        {
            //TODO Where to check for the equality of password and new password???
            if (ModelState.IsValid && 
                credentials.ConfirmPassword != null)
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        if (repository.CreateEntry(credentials).Result)
                        {
                            //201 The registation was added to the database,
                            return Created($"api/registration/{credentials}", credentials);
                        }
                        //403 User could not be added to the database.
                        return Forbid("User could not be added to the database.");
                    }
                    //300 User is already stored in the database.
                    return Redirect("User is already stored in the database.");
                }
                catch (Exception ex)
                {
                    logger.LogError($"DB request failed. {ex.Message}");
                    //TODO Implement the error redirection
                    return Redirect("/error");
                }
            }
            //400
            return BadRequest();
        }        
    }
}
