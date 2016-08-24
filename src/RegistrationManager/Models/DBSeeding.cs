using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public class DbSeeding
    {
        private DbManagerContext dbContext;

        public DbSeeding(DbManagerContext dbManagerContext)
        {
            dbContext = dbManagerContext;
        }

        public async Task EnsureSeedData()
        {
            if (!dbContext.DbCredentials.Any())
            {
                var credentialsDefault = new Credential()
                {
                    ID = 0,
                    Email = "vdettling@web.de",
                    NewPassword = "Default1234",
                    ConfirmPassword = "Default1234"
                };

                dbContext.Add(credentialsDefault);

                //Push the default data to the actual DB.
                await dbContext.SaveChangesAsync();
            }

        }

    }
}