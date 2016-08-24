using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RegistrationManager.Controllers;
using RegistrationManager.Models;

namespace RegistrationManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Injects also the config.json file, for example for the DB connection.
            services.AddSingleton(Configuration);
            // Add framework services.
           
            services.AddMvc().AddJsonOptions(config =>
            {   //All Json file formates will be resulved via camel cases.  
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //Injection of the IRegistrationRepository for later testing
            services.AddScoped<IRegistrationsRepository, RegistrationsRepository>();

            //Register DB
            services.AddDbContext<DbManagerContext>();

            //Inject DBSeeding if needed
            services.AddTransient<DbSeeding>();

            //Injects logging possibility
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            DbSeeding seeder,
            ILoggerFactory loggerFactory)
        {
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Index", action = "Interfaces" });
            });

#if DEBUG   //Only while debuging
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Information);
            loggerFactory.AddDebug();
#endif            
            //Last call becaus synchronised.
            seeder.EnsureSeedData().Wait();
        }
    }
}
