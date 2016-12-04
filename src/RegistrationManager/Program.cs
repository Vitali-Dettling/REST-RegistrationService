using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace RegistrationManager
{
    /// <summary>
    /// Contains the main class and the host configuration.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main file as well as configuration file about the host environment.
        /// Moreover, one can define the directories and the Startup.cs file.
        /// </summary>
        /// <param name="args">
        /// In order to pass additional external parameters, which is currently not used. 
        /// </param>
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")//Provides detailed information about the API
                .Build();

            host.Run();
        }
    }
}
