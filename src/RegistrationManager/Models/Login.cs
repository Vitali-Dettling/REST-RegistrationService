using Newtonsoft.Json;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationManager.Models
{
    /// <summary>
    /// Container in order to check the login details of an user. 
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Attribure in order to store the email detail of an user. 
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonProperty("Email")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        /// <summary>
        /// Attribure in order to store the password detail of an user. 
        /// </summary>
        [Required]
        [JsonProperty("Password")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password { get; set; }
    }
}
