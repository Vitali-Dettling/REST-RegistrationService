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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace RegistrationManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json")
                 .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)                
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)// TODO* Bug is not working any more? ->, IHostingEnvironment environment)
        {
            //Injects also the config.json file, for example for the DB connection.
            services.AddSingleton(Configuration);
            // Add framework services.
           
            services.AddMvc(config =>
            {
#if !DEBUG //TODO* Has to change to IHostingEnvironment environment used to work but sudenly not any more, see above.
            //  if (environment.IsProduction())
                {
                    //Redirects to a https, only used in productions
                    config.Filters.Add(new RequireHttpsAttribute());
                }
#endif
            })
            .AddJsonOptions(config =>
            {   //All Json file formates will be resulved via camel cases.  
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //Inject the user identity framwork. Moreover, here one can config the required identification details.
            services.AddIdentity<UserIdentity, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<DbManagerContext>();

            //Injection of the IRegistrationRepository for later testing
            services.AddScoped<IRegistrationsRepository, RegistrationsRepository>();

            //Register DB
            services.AddDbContext<DbManagerContext>();

            //Inject DBSeeding if needed
            services.AddTransient<DbSeeding>();

            //Injects logging possibility
            services.AddLogging();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// Note: the order matters to ensure which framework will be called first.
        /// </summary>
        /// <param name="app">
        /// The service which is proveded
        /// </param>
        /// <param name="seeder">
        /// Initialize the database, in case no entry exists.
        /// </param>
        /// <param name="loggerFactory">
        /// Logs the process of the service, in case of an error.
        /// </param>
        /// <param name="environment">
        /// Environment variable in order to ensure whether it is a development environment or a runtime environment.
        /// </param>
        public void Configure(IApplicationBuilder app, 
            DbSeeding seeder,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment)
        {
            //Only while debuging
            if (environment.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Information);
                loggerFactory.AddDebug();
                //More presise error information
                app.UseDeveloperExceptionPage();
            }

            //The service is using the Identity framework.
            app.UseIdentity();

            //Should be one of the last calls.
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Index", action = "Interfaces" });
            });

            //Last call because synchronised
            seeder.EnsureSeedData().Wait();
        }
    }
}
