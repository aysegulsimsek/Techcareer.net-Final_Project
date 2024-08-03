using DBlog.Data.Abstract;
using DBlog.Data;
using DBlog.Entity;
using DBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;


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
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Home", "Article");
            }
            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Home", "Article");
            }
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Name = model.Name,
                        Password = model.Password,
                        Image = "p1.jpg"
                    });
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Username ya da Email kullanımda.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

                if (isUser != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, isUser.UserName ?? ""),
                        new Claim(ClaimTypes.GivenName, isUser.Name ?? ""),
                        new Claim(ClaimTypes.UserData, isUser.Image ?? "")
                    };

                    if (isUser.Email == "info@asimsek.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var autProperties = new AuthenticationProperties { IsPersistent = true };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), autProperties
                    );
                    return RedirectToAction("Home", "Article");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya parola hatalı");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userRepository.Users
                .Include(x => x.Articles)
                .Include(x => x.Comments)
                .ThenInclude(c => c.Article)
                .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.Name;
            }

            var user = await _userRepository.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userRepository.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);

                if (user != null)
                {

                    if (!string.IsNullOrEmpty(model.Name) && model.Name != user.Name)
                    {
                        user.Name = model.Name;
                    }


                    if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
                    {
                        user.Email = model.Email;
                    }


                    if (!string.IsNullOrEmpty(model.Password) && model.Password != user.Password)
                    {
                        user.Password = model.Password;
                    }


                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        // Yeni resim kaydet
                        var imagePath = Path.Combine("wwwroot/img", model.ImageFile.FileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }
                        user.Image = model.ImageFile.FileName;
                    }

                    _userRepository.Update(user);
                    await _userRepository.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";


                    return RedirectToAction("Profile", "Users", new { username = user.UserName });
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                }
            }

            return View(model);
        }
    }
}

