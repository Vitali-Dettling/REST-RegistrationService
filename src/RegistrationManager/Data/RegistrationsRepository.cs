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
    /// <summary>
    /// DB queries will be implemented in the repository call (pattern).
    /// </summary>
    public class RegistrationsRepository : IRegistrationsRepository
    {
        private DbManagerContext dbContext;
        private UserManager<DbUserIdentity> userManagerIdentity;

        public RegistrationsRepository(DbManagerContext dbManagerContext,
            UserManager<DbUserIdentity> userMangerIdentity)
        {
            dbContext = dbManagerContext;
            userManagerIdentity = userMangerIdentity;
        }

        public async Task<Boolean> CreateEntry(Register newRegistration)
        {
            //TODO Make that better, is realy redundant.
            var user = new DbUserIdentity()
            {
                UserName = newRegistration.Email,
                Email = newRegistration.Email,
            };
            
            //Stores the default user infromation in the database
            var result = await userManagerIdentity.CreateAsync(user, newRegistration.Password);

            if (result.Succeeded)
            {
                return true;
            }            
            return false;
        }
    }
}