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

namespace RegistrationManager.Controllers.Api
{
    [Route("api/")]
    public class RegistrationController : Controller
    {
        //TODO Using the logger
        private ILogger<RegistrationController> logger;
        private IRegistrationsRepository repository;
        private SignInManager<UserIdentity> signInMgt;

        public RegistrationController(DbManagerContext dbMangerInjection, 
            ILogger<RegistrationController> loggerFactory, 
            IRegistrationsRepository registrationRepository,
            SignInManager<UserIdentity> signInManager)
        {
            //DB queries will be implemented in the repository call (pattern).
            repository = registrationRepository;
            logger = loggerFactory;
            signInMgt = signInManager;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credential credentials)
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
                    return Redirect("/error");
                }
            }
            //400
            return BadRequest();
        }

        [HttpPost("registration")]
        public IActionResult Post([FromBody] Credential credentials)
        {
            if (ModelState.IsValid && 
                credentials.ConfirmPassword != null)
            {
                try
                {
                    if (repository.CreateEntry(credentials).Result)
                    {
                        //201
                        return Created($"api/registration/{credentials}", credentials);
                    }
                    //403
                    return Forbid();
                }
                catch (Exception ex)
                {
                    logger.LogError($"DB request failed. {ex.Message}");
                    return Redirect("/error");
                }
            }
            //400
            return BadRequest();
        }

        [HttpGet("registrations")]
        public IActionResult Get()
        {
            try
            {
                var allRegistrations = repository.GetAllCredentials();
                //200
                return Ok(allRegistrations);
            }
            catch (Exception ex)
            {
                logger.LogError($"Faild to get all registrations {ex.Message}");
                return Redirect("/error");
            }
        }
    }
}
