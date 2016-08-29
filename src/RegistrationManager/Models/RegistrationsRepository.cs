using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private DbManagerContext dbContext;

        public RegistrationsRepository(DbManagerContext dbManagerContext)
        {
            dbContext = dbManagerContext;
        }

        public IEnumerable<Credential> GetAllCredentials()
        {
            return dbContext.DbCredentials.ToList();
        }

        public async Task CreateEntry(Credential newCredentials)
        { 
            dbContext.Add(newCredentials);
            //Push the default data to the actual DB.
            await dbContext.SaveChangesAsync();
        }
    }
}