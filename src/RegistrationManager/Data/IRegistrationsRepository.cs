using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Repository pattern interface, in order to manage the database. 
    /// </summary>
    public interface IRegistrationsRepository
    {
        /// <summary>
        /// Create a new entry (row) in the database by means of the entity framework. 
        /// </summary>
        /// <param name="newRegistration">
        /// Required information about the user, e.g. email, password, etc. 
        /// </param>
        /// <returns>
        /// Retrurns a true when the user was sored in the databse, otherwise false.  
        /// </returns>
        Task<Boolean> CreateEntry(Register newRegistration);
    }
}