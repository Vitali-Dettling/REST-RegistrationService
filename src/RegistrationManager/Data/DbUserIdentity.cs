using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Extents the database, where the user information are stored with additional properties.  
    /// </summary>
    public class DbUserIdentity : IdentityUser
    {
        /// <summary>
        /// Additional stored column in the database and the column type is DataTime.
        /// </summary>
        public DateTime Registration { get; set; }
    }
}