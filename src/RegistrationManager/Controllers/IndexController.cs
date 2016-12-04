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
    /// <summary>
    /// Manage the start page of the API in order to find out how to use the service. 
    /// </summary>
    public class IndexController : Controller
    {
        private ILogger<IndexController> logger;

        /// <summary>
        /// Constructor to inject the required classes and frameworks like the logger.
        /// </summary>
        /// <param name="loggerFactory">
        /// Logs all steps in case of an error.
        /// </param>
        public IndexController(ILogger<IndexController> loggerFactory)
        {
            logger = loggerFactory;
        }

        /// <summary>
        /// Index/start page of the service, in order to find out how to use the API.
        /// </summary>
        /// <returns>
        /// Returns the strat page which is managed via the swagger (Swashbuckle) framework
        /// </returns>
        public IActionResult API()
        {
            logger.LogInformation("Swagger framework is being called.");
            return View();
        }
    }
}
