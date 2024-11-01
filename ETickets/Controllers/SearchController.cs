using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMovieRepository movieRepository;

        public SearchController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }
        public IActionResult Index(string name)
        {
            var movies = movieRepository.Get(expression:e=>e.Name.Contains(name));
            return View(movies);
        }
    }
}
