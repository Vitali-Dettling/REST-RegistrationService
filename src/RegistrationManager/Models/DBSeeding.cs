using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public class DbSeeding
    {
        private DbManagerContext dbContext;
        private IRegistrationsRepository repository;
        private UserManager<DbUserIdentity> userManagerIdentity;

        public DbSeeding(DbManagerContext dbManagerContext, 
            IRegistrationsRepository registrationRepository,
            UserManager<DbUserIdentity> userMangerIdentity)
        {
            dbContext = dbManagerContext;
            repository = registrationRepository;
            userManagerIdentity = userMangerIdentity;
        }

        public async Task EnsureSeedData()
        {
            //Check whether the default user exists.
            if (await userManagerIdentity.FindByEmailAsync("vdettling@web.de") == null)
            {
                var user = new DbUserIdentity()
                {
                    UserName = "vdettling@web.de",
                    Email = "vdettling@web.de",
                };

                //Stores the default user infromation in the database
                var result = await userManagerIdentity.CreateAsync(user, "P@ssw0rd!");
                if (!result.Succeeded)
                {
                    throw new TaskSchedulerException("Seeding of default user was not successful.");
                }
            }
        }
    }
}