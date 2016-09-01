using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<Boolean> CreateEntry(Credential newCredentials)
        {
            if (!CheckDBEntries(newCredentials))
            {
                dbContext.Add(newCredentials);
                //Push the default data to the actual DB.
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether an dublicate is stored in the DB. 
        /// If that is the case, then the user will not be stored a second time. 
        /// </summary>
        /// <param name="newCredentials">
        /// Required credentials of an user in order to be registered.
        /// </param>
        /// <returns>
        /// True if an entry exist in the DB.
        /// </returns>
        public Boolean CheckDBEntries(Credential newCredentials)
        {
            //TODO Iteration trough the DB is to slow, needs to be improved, maybe by LINQ to SQL? 
            var check = false;
            foreach (var credential in dbContext.DbCredentials)
            {
                if (credential.Email.Equals(newCredentials.Email) && 
                    credential.Password.Equals(newCredentials.Password))
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        public IEnumerable<Credential> GetAllCredentials()
        {
            return dbContext.DbCredentials.ToList();
        }
    }
}