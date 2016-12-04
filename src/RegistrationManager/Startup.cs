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
    /// <summary>
    /// This is class contains all configuration methods and properties. 
    /// The class is divided into three parts: 
    /// Firstly, the constructor
    /// Secondly, the ConfigureServices method, to specify the service itself
    /// Thirdly, the Configure method, to specify the frameworks and to start the database seeding 
    /// </summary>
    public class Startup
    {
        private IHostingEnvironment environment;

        /// <summary>
        /// Constructor of startup.cs class, in oder to specify the hosting environment.
        /// </summary>
        /// <param name="env">
        /// Hosting environment in which the service will be held, 
        /// as well as to distinguish between developer and hosting enviroment.
        /// </param>
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
            environment = env;

        }

        /// <summary>
        /// Returns the configuration property about the service. It is not beeing used within the service itself.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// Specificytion of how the injection should be called, 
        /// as well as the configuration of the injections.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Injects also the config.json file, for example for the DB connection.
            services.AddSingleton(Configuration);
            
            //Injects MCV pattern.
            services.AddMvc(config =>
            {
            if (environment.IsProduction())
                {
                    //Redirects to a https, only used in productions
                    //TODO Need to be adjusted in order to get a secure https connection. 
                    //config.Filters.Add(new RequireHttpsAttribute());
                }

            })
            .AddJsonOptions(config =>
            {   //All Json file formates will be resulved via camel cases.  
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            //Inject the user identity framwork. Moreover, here one can config the required identification details.
            services.AddIdentity<DbUserIdentity, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 6;
                //TODO Do I realy need this, because the RegistrationManger API does not work with cookies anyway?
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
            })
            .AddEntityFrameworkStores<DbManagerContext>();

            //Inject an implementation of ISwaggerProvider with defaulted settings applied
            services.AddSwaggerGen(config =>
            {
                //Swagger API reads the class, method, etc. descriptions by means of the xml file.
                //TODO the path is hard coded needs to be a relative path?
                if (environment.IsProduction())
                {
                    config.IncludeXmlComments(environment.ContentRootPath + "/RegistrationManager.xml");
                }
                else
                {
                    config.IncludeXmlComments(environment.ContentRootPath + "/bin/Debug/netcoreapp1.0/RegistrationManager.xml");
                }
            }
            );

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

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
            
            //Should be one of the last calls.
            app.UseMvc(config =>
            {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Index", action = "API" });
            });

            //Last call because synchronised
            seeder.EnsureSeedData().Wait();
        }
    }
}
