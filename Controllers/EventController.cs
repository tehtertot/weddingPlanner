using Microsoft.AspNetCore.Mvc;
using wedding.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace wedding.Controllers
{
    public class EventController : Controller 
    {
        private WeddingContext _context;
        public EventController(WeddingContext context)
		{
			_context = context;
		}

        [HttpGet]
        [Route("dashboard")]
        //display the dashboard showing all weddings and the option to delete/rsvp/un-rsvp
        public IActionResult Dashboard()
        {
            //check whether user is logged in, and if so get session variables (user id and name)
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) {
                return RedirectToAction("Index", "User");
            }
            ViewBag.userid = userId;
            ViewBag.username = HttpContext.Session.GetString("Username");
            
            //grab all weddings to show on DB
            ViewBag.weddings = _context.Weddings.Include(w => w.Guests).ToList();

            //create a list of all the weddings IDs the current user is attending
            List<Guest> guests = _context.Guests.Where(g => g.UserId == userId).ToList();
            List<int> weddingsAttending = new List<int>();
            foreach (Guest g in guests) {
                weddingsAttending.Add(g.WeddingId);
            }
            ViewBag.attending = weddingsAttending;
            return View();
        }

        [HttpGet]
        [Route("add")]
        //show the page to add a wedding
        public IActionResult ShowAdd() {
            //check whether user is logged in, and if so get session variables (user id and name)
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) {
                return RedirectToAction("Index", "User");
            }
            ViewBag.userid = userId;
            ViewBag.username = HttpContext.Session.GetString("Username");
            return View("Add");
        }

        [HttpPost]
        [Route("create")]
        //validate the submission and add the wedding to the db
        public IActionResult CreateWedding(Wedding wed) {
            //check whether user is logged in, and if so get session variables (user id and name)
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) {
                return RedirectToAction("Index", "User");
            }
            if (ModelState.IsValid) {
                //set current user as wedding creator and add to db
                wed.UserId = (int)userId;
                _context.Add(wed);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            //show add page with errors
            return View("Add");
        }

        [HttpGet]
        [Route("delete/{id}")]
        //delete a given wedding from the db -- UPDATE TO POST ROUTE
        public IActionResult DeleteEvent(int id) {
            Wedding wed = _context.Weddings.Where(w => w.WeddingId == id).SingleOrDefault();
            int? userId = HttpContext.Session.GetInt32("UserId");
            //validate the user deleting is the user who created the wedding
            if (userId == wed.UserId) {
                _context.Weddings.Remove(wed);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("show/{id}")]
        //show a specific wedding with details and map
        public IActionResult Show(int id) {
            //check whether user is logged in, and if so get session variables (user id and name)
            
            //grab wedding details
            ViewBag.wedding = _context.Weddings.Where(w => w.WeddingId == id).Include(w => w.Guests).ThenInclude(g => g.User).SingleOrDefault();
            //and guest list
            // ViewBag.guests = _context.Guests.Where(g => g.WeddingId == id).Include(g => g.User);
            return View();
        }

        [HttpGet]
        [Route("unrsvp/{wedid}")]
        //remove user from guest list for the given wedding -- UPDATE TO POST ROUTE
        public IActionResult UNRSVP(int wedid) {
            //check whether user is logged in, and if so get session variables (user id and name)
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) {
                return RedirectToAction("Index", "User");
            }
            //instantiate guest to then delete from db
            Guest toDelete = _context.Guests.Where(g => g.WeddingId == wedid && g.UserId == userId).SingleOrDefault();
            _context.Guests.Remove(toDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("rsvp/{wedid}")]
        //add user to guest list for the given wedding -- UPDATE TO POST ROUTE
        public IActionResult RSVP(int wedid) {
            //check whether user is logged in, and if so get session variables (user id and name)
            int? uId = HttpContext.Session.GetInt32("UserId");
            if (uId == null) {
                return RedirectToAction("Index", "User");
            }
            //create guest and add to db
            Guest newGuest = new Guest {
                WeddingId = wedid,
                UserId = (int)uId
            };
            _context.Guests.Add(newGuest);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}