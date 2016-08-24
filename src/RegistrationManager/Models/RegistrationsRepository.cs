using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}