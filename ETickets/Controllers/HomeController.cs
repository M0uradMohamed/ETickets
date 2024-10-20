using ETickets.Data;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ETickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        ApplicationDbContext context = new ApplicationDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = context.Movies.Include(m => m.category ).Include(m=>m.cinema ).ToList();
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var movie = context.Movies.Include(m=>m.category).Include(m=>m.cinema)
                .Include(m=>m.Actors).Where(m=>m.Id==id).FirstOrDefault();
            var actorsmovies = context.ActorsMovies.Where(e=>e.MovieId ==id ).Select( e=> new { e.ActorId , e.MovieId}).ToList();
            ViewBag.actorsmovies = actorsmovies;
            var actors = context.Actors.Select( e=> new { FullName= e.FirstName+" "+e.LastName , e.Bio , e.Id , e.ProfilePicture , e.News , e.Movies } ).ToList();
            ViewBag.actors = actors;
            return View(movie);
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
