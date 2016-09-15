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

        /// <summary>
        /// Repository patter in order to manage everything that is related to the database.
        /// </summary>
        /// <param name="dbManagerContext">
        /// Constructore injection of the database management context.
        /// </param>
        /// <param name="userMangerIdentity">
        /// Constructor injection of the user management identity.
        /// </param>
        public RegistrationsRepository(DbManagerContext dbManagerContext,
            UserManager<DbUserIdentity> userMangerIdentity)
        {
            dbContext = dbManagerContext;
            userManagerIdentity = userMangerIdentity;
        }

        /// <summary>
        /// Creates a new entry in the database. 
        /// The User Manager is required in order to store all important imformation about the user. 
        /// </summary>
        /// <param name="newRegistration">
        /// Required information about the user, e.g. email, password, etc. 
        /// </param>
        /// <returns>
        /// Retrurns a true when the user was stored in the databse, otherwies false. 
        /// </returns>
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