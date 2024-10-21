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
            var allMovies = context.Movies.ToList();
            foreach (var movie in allMovies)
            {
                if(movie.StartDate > DateTime.Now)
                    movie.movieStatus = MovieStatus.Upcoming;
                else if(movie.StartDate <= DateTime.Now && movie.EndDate >= DateTime.Now)
                    movie.movieStatus = MovieStatus.Available;
                else
                    movie.movieStatus = MovieStatus.Expired;
            }
 
            context.SaveChanges();

            var movies = context.Movies.Include(m => m.category ).Include(m=>m.cinema ).ToList();
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var acotrsmovies = context.Movies.Include(e => e.Actors)
                .Include(e => e.category).Include(e => e.cinema).Where(e => e.Id == id).FirstOrDefault();
            return View(acotrsmovies);
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
