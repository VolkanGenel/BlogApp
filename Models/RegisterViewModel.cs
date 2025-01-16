using System.ComponentModel.DataAnnotations;
using BlogApp.Entity;

namespace BlogApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Adress")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "{0} field need to be at least {2}, maximum {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Valid Password")]
        public string? Password { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "{0} field need to be at least {2}, maximum {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords require to be same!")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}