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
    public class WeddingController : Controller
    {
        private MyContext dbContext;
        public WeddingController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("/add")]
        public IActionResult AddWeddingPage()
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                return View();
            }
            return View("Index", "Home");
        }

        [HttpPost]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if (ModelState.IsValid)
            {
                newWedding.UserID = (int)HttpContext.Session.GetInt32("UserID");
                dbContext.Add(newWedding);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingHomePage", "Home");
            }
            return View("AddWeddingPage");
        }

        [HttpGet("/details/{WeddingID}")]
        public IActionResult WeddingDetailsPage(int WeddingID)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                Wedding DisplayedWedding = dbContext.Weddings
                .Where(w => w.WeddingID == WeddingID)
                .Include(G => G.Attendees)
                .ThenInclude(u => u.User)
                .FirstOrDefault();
                return View(DisplayedWedding);
            }
            return RedirectToAction("WeddingHomePage", "Home");
        }

        [HttpPost]
        public IActionResult RSVP(WeddingUserVM model)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                Guest guest = model.Guest;
                dbContext.Add(guest);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingHomePage", "Home");

            }
            return RedirectToAction("WeddingHomePage", "Home");
        }

        [HttpPost("cancel/{WeddingID}")]
        public IActionResult Un_RSVP(int WeddingID)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                Guest G = dbContext.Guests
                .Where(W => W.WeddingID == WeddingID && W.UserID == (int) HttpContext.Session.GetInt32("UserID"))
                .FirstOrDefault();
                dbContext.Guests.Remove(G);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingHomePage", "Home");
            }
            return RedirectToAction("WeddingHomePage", "Home");
        }

        [HttpPost("delete/{WeddingID}")]
        public IActionResult Delete(int WeddingID)
        {
            if (HttpContext.Session.GetInt32("UserID") != null)
            {
                Wedding W = dbContext.Weddings
                .Where(w => w.WeddingID == WeddingID)
                .Include(G => G.Attendees)
                .ThenInclude(u => u.User)
                .FirstOrDefault();
                dbContext.Weddings.Remove(W);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingHomePage", "Home");
            }
            return RedirectToAction("WeddingHomePage", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}