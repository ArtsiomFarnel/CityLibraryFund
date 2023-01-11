using CityLibraryFund.Models;
using CityLibraryFund.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CityLibraryFund.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseContext _context;

        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u =>
                    u.Name == loginModel.Name && u.Password == loginModel.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Неверное имя или пароль!");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == registerModel.Name);
            if (user == null)
            {
                Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "client");
                user = new User
                {
                    Name = registerModel.Name,
                    Password = registerModel.Password,
                    Role = userRole,
                    DateOfRegistration = DateTime.Now,
                    Email = "test" //must be deleted
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await Authenticate(user);
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Неверное имя или пароль!");
            return View(registerModel);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = 
                new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
