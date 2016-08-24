using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RegistrationManager.Models
{
    public class DbManagerContext : DbContext
    {
        private IConfigurationRoot config;

        public DbManagerContext(IConfigurationRoot configurationRoot, DbContextOptions options)
            :base(options)
        {
            config  = configurationRoot;
        }

        public DbSet<Credential> DbCredentials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(config["ConnctionStrings:DBConnectionContext"]);
        }
    }
}
