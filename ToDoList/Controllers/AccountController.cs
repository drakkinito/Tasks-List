using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using ToDoList.ViewModels;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Linq;

namespace ToDoList.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ListContext _db;

        public AccountController(ListContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users
                        .FirstOrDefaultAsync(u =>
                            u.Email == model.Email && VerifyHashedPassword(model.Password, u.Password));


                if (user != null)
                {
                    await Authenticate(user);
                    UsersGroup usersGroup = _db.UsersGroups
                            .FirstOrDefault(x => x.GroupItemId == user.Id);

                    return RedirectToAction("Index", "Home", new { id = usersGroup.GroupItemId });

                    //HttpContext.Response.Cookies.Append("user_id", $"{user.Id}");
                    //await Authenticate(model.Email);

                }
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                string password = HashPassword(model.Password, model.Email);

                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    user = _db.Users.Add(new User { Email = model.Email, Password = password }).Entity;

                    await _db.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    InitializeData.Initialize(_db, user);

                    //HttpContext.Response.Cookies.Append("user_id", $"{user.Id}");

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }

        // Learn
        //private async Task Authenticate(string userName)
        //{
        //    // создаем один claim
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
        //    };
        //    // создаем объект ClaimsIdentity
        //    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //    // установка аутентификационных куки
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}

        public async Task Authenticate(User user)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim("sub", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }, CookieAuthenticationDefaults.AuthenticationScheme)));
        }

        public async Task<IActionResult> Logout()
        {
            //HttpContext.Response.Cookies.Delete("user_id");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public string HashPassword(string password, string user)
        {
            const int Pbkdf2Iterations = 1000;
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = Encoding.UTF8.GetBytes(user);

            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, 20);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        public bool VerifyHashedPassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[0]);
            var salt = Convert.FromBase64String(split[1]);
            var hash = Convert.FromBase64String(split[2]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}