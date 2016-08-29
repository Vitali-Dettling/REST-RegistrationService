using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public class DbSeeding
    {
        private DbManagerContext dbContext;
        private IRegistrationsRepository repository;

        public DbSeeding(DbManagerContext dbManagerContext, 
            IRegistrationsRepository registrationRepository)
        {
            dbContext = dbManagerContext;
            repository = registrationRepository;
        }

        public async Task EnsureSeedData()
        {
            if (!dbContext.DbCredentials.Any())
            {
                var credentialsDefault = new Credential()
                {
                    ID = 0000000,
                    Email = "vdettling@web.de",
                    Password = "Default1234",
                    ConfirmPassword = "Default1234"
                };

                await repository.CreateEntry(credentialsDefault);
            }

        }

    }
}