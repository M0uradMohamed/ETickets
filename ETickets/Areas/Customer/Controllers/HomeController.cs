using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepository movieRepository;


        public HomeController(IMovieRepository movieRepository, ILogger<HomeController> logger)
        {
            this.movieRepository = movieRepository;
            _logger = logger;
        }

        // ApplicationDbContext context = new ApplicationDbContext();
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            var allMovies = movieRepository.Get();
            foreach (var movie in allMovies)
            {
                if (movie.StartDate > DateTime.Now)
                    movie.MovieStatus = MovieStatus.Upcoming;
                else if (movie.StartDate <= DateTime.Now && movie.EndDate >= DateTime.Now)
                    movie.MovieStatus = MovieStatus.Available;
                else
                    movie.MovieStatus = MovieStatus.Expired;
            }

            movieRepository.Commit();

            var movies = movieRepository.Get(includeProps: [m => m.Category, m => m.Cinema]);
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var acotrsmovies = movieRepository.GetOne(includeProps: [e => e.Actors
                , e => e.Category,e => e.Cinema ], expression: e => e.Id == id);
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