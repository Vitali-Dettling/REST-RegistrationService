using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Create a seed entry in the databse, when if was empty before. 
    /// </summary>
    public class DbSeeding
    {
        private IRegistrationsRepository repository;
        private UserManager<DbUserIdentity> userManagerIdentity;

        /// <summary>
        ///  Creates seeding information in the database, in case it had been empty before. 
        /// </summary>
        /// <param name="registrationRepository">
        /// Constructor injection of the repository pattern, in order to manage a new seeding entry in the databse.
        /// </param>
        /// <param name="userMangerIdentity">
        /// Constructor injection of the user manager, in order to retrieve information from the database.
        /// </param>
        public DbSeeding(IRegistrationsRepository registrationRepository,
            UserManager<DbUserIdentity> userMangerIdentity)
        {
            repository = registrationRepository;
            userManagerIdentity = userMangerIdentity;
        }

        /// <summary>
        /// Seed the database, when it was empty before. 
        /// </summary>
        /// <returns>
        /// The Task class is required for the Wait() operator while seeding the database. 
        /// </returns>
        public async Task EnsureSeedData()
        {
            //Check whether the default user exists.
            if (await userManagerIdentity.FindByEmailAsync("vdettling@web.de") == null)
            {
                //Just for seeding purpose. 
                var user = new Register()
                {
                    Email = "vdettling@web.de",
                    Password = "P@ssw0rd!"
                };

                //Stores the default user infromation in the database
                if (repository.CreateEntry(user).Result)
                {
                    throw new TaskSchedulerException("Seeding of default user was not successful.");
                }
            }
        }
    }
}