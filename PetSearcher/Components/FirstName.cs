using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetSearcher.Models;
using System.Security.Claims;

namespace PetSearcher.Components
{
    
    public class FirstName : ViewComponent
    {
        private UserManager<ApplicationUser> _userManager;

        public FirstName(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        

        public string Invoke()
        {
            if (User.Identity.IsAuthenticated)
                return _userManager.FindByNameAsync(User.Identity.Name).Result.FirstName;
            return "Guest";
        }
    }
}
