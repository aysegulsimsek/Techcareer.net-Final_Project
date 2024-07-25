using System.Security.Claims;
using DBlog.Data.Abstract;
using DBlog.Entity;
using DBlog.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DBlog.Controllers
{
    public class UsersController : Controller
    {

        private IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Home", "Article");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users
                    .FirstOrDefaultAsync(x => x.UserName == model.UserName);

                if (user != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new(ClaimTypes.Name, user.UserName ?? ""),
                        new(ClaimTypes.GivenName, user.Name ?? ""),
                        new(ClaimTypes.UserData, user.Image ?? "")
                    };

                    if (user.UserName == "admin")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties { IsPersistent = true };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Home", "Article");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya parola hatalı");
                }
            }
            return View(model);
        }


    }
}
