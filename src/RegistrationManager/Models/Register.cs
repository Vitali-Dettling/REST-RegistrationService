using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Swashbuckle.SwaggerGen.Annotations;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Container which inherents Login properties and extends them with the ConformPassword attribute.
    /// </summary>
    public class Register : Login
    {
        /// <summary>
        /// Attribure in order to store the confirm password detail of an user. 
        /// </summary>
        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword { get; set; }
    }
}
