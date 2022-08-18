using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetSearcher.Models;
using PetSearcher.Models.ViewModels;
using PetSearcher.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PetSearcher.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    
                    return RedirectToAction("Index", "Notice");
                }
                else
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }

        public  IActionResult Register()
        {
            //if(!_roleManager.RoleExistsAsync(HelperClass.Support).GetAwaiter().GetResult())
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(HelperClass.Support));
            //    await _roleManager.CreateAsync(new IdentityRole(HelperClass.User));
            //}    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Notice");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

    }

}
