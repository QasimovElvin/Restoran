using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.Models;
using Restoran.ViewModels.Account;

namespace Restoran.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            if(loginVM.EmailOrUsername.Contains("@"))
            {
                var user= await _userManager.FindByEmailAsync(loginVM.EmailOrUsername);
                if (user == null)
                {
                    ModelState.AddModelError("", "User is null");
                    return View();
                }
                var pass=await _signInManager.PasswordSignInAsync(user, loginVM.Password,false,false);
                if (!pass.Succeeded)
                {
                    ModelState.AddModelError("", "Password wrong");
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var user = await _userManager.FindByNameAsync(loginVM.EmailOrUsername);
                if (user == null)
                {
                    ModelState.AddModelError("", "User is null");
                    return View();
                }
                var pass = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                if (!pass.Succeeded)
                {
                    ModelState.AddModelError("", "Password wrong");
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
