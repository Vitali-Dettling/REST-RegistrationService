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
    public class DbManagerContext : IdentityDbContext<UserIdentity>
    {
        private IConfigurationRoot config;

        public DbManagerContext(IConfigurationRoot configurationRoot, DbContextOptions options)
            :base(options)
        {
            config  = configurationRoot;
        }

        public DbSet<Credential> DbCredentials { get; set; }

        public DbSet<UserIdentity> DbUserIdentity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(config["ConnctionStrings:DBConnectionContext"]);
            
        }
    }
}
