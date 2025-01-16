using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "{0} field need to be at least {2}, maximum {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Valid Password")]
        public string? Password { get; set; }
    }
}