using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Represents all types of data that will be stored in the database.
    /// </summary>
    public class DbManagerContext : IdentityDbContext<DbUserIdentity>
    {
        private IConfigurationRoot config;

        /// <summary>
        /// Constructor where the classes and frameworks are injected, in oder to manage the database connection. 
        /// </summary>
        /// <param name="configurationRoot">
        /// Configuration of the database, e.g. esablish the database connection.  
        /// </param>
        /// <param name="options">
        /// Database option can be spezified here, which then will be passed to the base class. 
        /// </param>
        public DbManagerContext(IConfigurationRoot configurationRoot, DbContextOptions options)
            :base(options)
        {
            config  = configurationRoot;
        }

        /// <summary>
        /// Database extension with additional user information. 
        /// </summary>
        public DbSet<DbUserIdentity> DbUserIdentity { get; set; }

        /// <summary>
        /// Esablish the connection to the database and it passes them to the base class.
        /// </summary>
        /// <param name="optionsBuilder">
        /// The option builder gets the connection string from the config.json file.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(config["ConnctionStrings:DBConnectionContext"]);
            
        }
    }
}
