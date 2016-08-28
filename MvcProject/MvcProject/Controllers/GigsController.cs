using Microsoft.AspNet.Identity;
using MvcProject.Models;
using MvcProject.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(g =>g.Artist)
                .Include(g =>g.Genre)
                .ToList();
            var viewModel = new HomeViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };

            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewModel.Genres = _context.Genres.ToList();
                return View("Create", ViewModel);
            }
                

            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = ViewModel.GetDateTime(),
                GenreId = ViewModel.Genre,
                Venue = ViewModel.Venue
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }
    }
}