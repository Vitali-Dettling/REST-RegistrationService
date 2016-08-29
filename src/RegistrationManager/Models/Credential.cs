

using System.ComponentModel.DataAnnotations;

namespace RegistrationManager.Models
{
    public class Credential
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        public string Password { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPassword { get; set; }

    }
}