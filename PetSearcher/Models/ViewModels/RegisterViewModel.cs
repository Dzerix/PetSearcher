using System.ComponentModel.DataAnnotations;

namespace PetSearcher.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30,ErrorMessage ="The {0} must be at least {2} characters long.",MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }
    }
}
