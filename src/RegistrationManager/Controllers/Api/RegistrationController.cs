using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using RegistrationManager.Models;
using Microsoft.Extensions.Logging;

namespace RegistrationManager.Controllers.Api
{
    [Route("api/")]
    public class RegistrationController : Controller
    {
        private ILogger<RegistrationController> logger;
        private IRegistrationsRepository repository;

        public RegistrationController(DbManagerContext dbMangerInjection, 
            ILogger<RegistrationController> loggerFactory, 
            IRegistrationsRepository registrationRepository)
        {
            //DB queries will be implemented in the repository call (pattern).
            repository = registrationRepository;
            logger = loggerFactory;
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] Credential credentials)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (repository.CheckDBEntries(credentials))
                    {
                        //200
                        return Ok($"api/login/");
                    }
                    //404
                    return NotFound();
                }
                catch (Exception ex)
                {
                    logger.LogError($"DB request failed. {ex.Message}");
                    return Redirect("/error");
                }
            }
            //400
            return BadRequest("Validation Failed");
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
            return BadRequest("Bad Request");
        }
    }
}
