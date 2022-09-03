using System.ComponentModel.DataAnnotations;

namespace PetSearcher.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }   
    }
}
