using Microsoft.AspNetCore.Identity;

namespace PetSearcher.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
