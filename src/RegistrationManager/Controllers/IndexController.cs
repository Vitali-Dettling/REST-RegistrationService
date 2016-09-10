using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegistrationManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationManager.Controllers
{
    public class IndexController : Controller
    {
        private ILogger<IndexController> logger;
        private IRegistrationsRepository repository;

        public IndexController(ILogger<IndexController> loggerFactory, IRegistrationsRepository registrationRepository)
        {
            //DB queries will be implemented in the repository call (pattern).
            repository = registrationRepository;
            logger = loggerFactory;
        }

        public IActionResult API()
        {
            logger.LogInformation("No API has been called yet.");
            //TODO: Defelaut page to get the interface descriptions.
            return View();
        }
    }
}
