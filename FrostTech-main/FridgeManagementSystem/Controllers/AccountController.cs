using FridgeManagementSystem.BLL.DTOs;
using FridgeManagementSystem.BLL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FridgeManagementSystem.Controllers
{
    public class AccountController(IUserService userService) : Controller
    {
        private IUserService _userService = userService;

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            var result = await _userService.Login(userLogin);
            ClaimsIdentity? identity = null;
            bool isAuthenticated = false;
            if (result != null)
            {
                if(!string.IsNullOrEmpty(result.RoleName))
                {
                    identity = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.Role, result.RoleName)
                    ], CookieAuthenticationDefaults.AuthenticationScheme);

                    if(!string.IsNullOrEmpty(result.FirstName) && !string.IsNullOrEmpty(result.LastName))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.FirstName + " " + result.LastName));
                    }

                    isAuthenticated = true;
                }

                if(isAuthenticated && identity != null)
                {
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                
            }


            ViewData["Message"] = "User login details incorrect, please retry!!";

            return View();

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RequestAccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return View(accountDto);
            }

            await _userService.Register(accountDto);

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
