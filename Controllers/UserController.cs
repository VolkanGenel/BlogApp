using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Post");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                   var userClaims = new List<Claim>();
                   var claimId = new Claim(ClaimTypes.NameIdentifier, isUser.UserId.ToString());
                   var claimUserName = new Claim(ClaimTypes.Name, isUser.UserName ?? "");
                   var claimName = new Claim(ClaimTypes.GivenName, isUser.Name ?? "");
                   var claimImage = new Claim(ClaimTypes.UserData, isUser.Image ?? "");

                   userClaims.Add(claimId); 
                   userClaims.Add(claimUserName); 
                   userClaims.Add(claimName); 
                   userClaims.Add(claimImage); 

                    if(isUser.Email == "info@volkangenel.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                   return RedirectToAction("Index","Post"); 
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
              var user = await _userRepository.Users.FirstOrDefaultAsync(x=> x.UserName == model.UserName || x.Email == model.Email);
              if (user == null){
                _userRepository.CreateUser(new Entity.User {
                    UserName = model.UserName,
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Image = "avatar.jpeg"
                });
                return RedirectToAction("Login"); 
              } 
              else
              {
                ModelState.AddModelError("","Username or Email has been used by another User");
              }       
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
