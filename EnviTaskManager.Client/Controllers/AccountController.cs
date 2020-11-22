using EnviTaskManagerClient.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EnviTaskManagerClient.Controllers
{
    public class AccountController : Controller
    {
        

            private readonly UserManager<IdentityUser> _userManager;
            private readonly SignInManager<IdentityUser> _signInManager;

            public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            
         

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            public IActionResult AccessDenied()
            {
                return RedirectToAction("Index", "Home");
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fejl ved login");
                }
                return View(model);
            }

            //[HttpPost]
            public async Task<IActionResult> Logout()

            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

        }

    
}
