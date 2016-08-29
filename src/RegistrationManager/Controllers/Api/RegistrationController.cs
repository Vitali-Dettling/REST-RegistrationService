using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using RegistrationManager.Models;
using Microsoft.Extensions.Logging;
using RegistrationManager.Validations;

namespace RegistrationManager.Controllers.Api
{
    [Route("api/")]
    public class RegistrationController : Controller
    {
        private ILogger<RegistrationController> logger;
        private IRegistrationsRepository repository;
        private IValidate validation;

        public RegistrationController(DbManagerContext dbMangerInjection, 
            ILogger<RegistrationController> loggerFactory, 
            IRegistrationsRepository registrationRepository)
        {
            //DB queries will be implemented in the repository call (pattern).
            repository = registrationRepository;
            logger = loggerFactory;

            validation = new Validation();
        }
        
        [HttpGet("registrations")]
        public IActionResult Get()
        {
            try
            {
                var allRegistrations = repository.GetAllCredentials();
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
            if (validation.Login(credentials))
            {
                return Ok();
            }
            return BadRequest("Validation Failed");
        }

        [HttpPost("registration")]
        public IActionResult Post([FromBody] Credential credentials)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    repository.CreateEntry(credentials);
                    return Created($"api/registration/{credentials}", credentials);
                }
                catch (Exception ex)
                {
                    logger.LogError($"DB request failed. {ex.Message}");
                    return Redirect("/error");
                }
            }
            return BadRequest("Bad Request");
        }
    }
}
