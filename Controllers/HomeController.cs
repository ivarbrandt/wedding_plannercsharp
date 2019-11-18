using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using weddingplanner.Models;

namespace weddingplanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserLoginVM user)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(i => i.Email == user.NewUser.Email))
                {
                    ModelState.AddModelError("NewUser.Email", "Email already exists");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.NewUser.Password = Hasher.HashPassword(user.NewUser, user.NewUser.Password);
                dbContext.Add(user.NewUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserID", user.NewUser.UserID);
                return RedirectToAction("WeddingHomePage");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login(UserLoginVM loggeduserdata)
        {
            if (ModelState.IsValid)
            {
                User userdb = dbContext.Users.FirstOrDefault(e => e.Email == loggeduserdata.LoggedUser.LoginEmail);
                if (userdb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email");
                    return View("Index");
                }
                var hasher = new PasswordHasher<LoginCredentials>();
                var result = hasher.VerifyHashedPassword(loggeduserdata.LoggedUser, userdb.Password, loggeduserdata.LoggedUser.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserID", userdb.UserID);
                return RedirectToAction("WeddingHomePage");
            }
            return View("Index");
        }

        [HttpGet("/Dashboard")]
        public IActionResult WeddingHomePage()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                WeddingUserVM DisplayWeddings = new WeddingUserVM()

                {
                    AllWeddings = dbContext.Weddings
                    .Include(wed => wed.Attendees)
                    .ThenInclude(user => user.User)
                    .ToList(),

                    User = dbContext.Users
                    .Where(r => r.UserID == (int)HttpContext.Session.GetInt32("UserID"))
                    .Include(W => W.Guests)
                    .ThenInclude(w => w.Wedding)
                    .FirstOrDefault()
                };
                return View(DisplayWeddings);
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}