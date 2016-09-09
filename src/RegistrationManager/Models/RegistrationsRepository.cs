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
            return false;
        }
    }
}