using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace RegistrationManager.Models
{
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private DbManagerContext dbContext;
        private UserManager<UserIdentity> userManagerIdentity;

        public RegistrationsRepository(DbManagerContext dbManagerContext,
            UserManager<UserIdentity> userMangerIdentity)
        {
            dbContext = dbManagerContext;
            userManagerIdentity = userMangerIdentity;
        }

        public async Task<Boolean> CreateEntry(Credential newCredentials)
        {
            //TODO Make that better, is realy redundant.
            var user = new UserIdentity()
            {
                UserName = newCredentials.Email,
                Email = newCredentials.Email,
            };

            //Stores the default user infromation in the database
            var result = await userManagerIdentity.CreateAsync(user, newCredentials.Password);

            if (result.Succeeded)
            {
                return true;
            }
            //dbContext.Add(newCredentials);
            ////Push the default data to the actual DB.
            //await dbContext.SaveChangesAsync();
            
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
            //TODO Delete ???
            ////TODO Iteration through the DB is to slow, needs to be improved, maybe by LINQ to SQL? 
            //var check = false;

            //    foreach (var credential in dbContext.DbCredentials)
            //{
            //    if (credential.Email.Equals(newCredentials.Email) &&
            //        credential.Password.Equals(newCredentials.Password))
            //    {
            //        check = true;
            //        break;
            //    }
            //}
            //return check;
            return true;
        }

        public IEnumerable<Credential> GetAllCredentials()
        {
            return dbContext.DbCredentials.ToList();
        }
    }
}