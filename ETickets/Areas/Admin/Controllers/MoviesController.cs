using ETickets.Repository.IRepository;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using ETickets.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using ETickets.Repository;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IActorRepository actorRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public MoviesController(IMovieRepository movieRepository, ICinemaRepository cinemaRepository
            , ICategoryRepository categoryRepository ,IActorRepository actorRepository 
            ,IActorMovieRepository actorMovieRepository )
        {
            this.movieRepository = movieRepository;
            this.cinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
            this.actorRepository = actorRepository;
            this.actorMovieRepository = actorMovieRepository;
        }
        public IActionResult Index()
        {
            var movies = movieRepository.Get(includeProps: [e => e.Category, e => e.Cinema]);
            return View(movies);
        }
        public IActionResult create()
        {
            var categories = categoryRepository.Get();
            var cinemas = cinemaRepository.Get();
            var actors = actorRepository.Get();
            ViewBag.cinemas = cinemas;
            ViewBag.categories = categories;
            ViewBag.actors = actors;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(MovieVM movieVM, IFormFile file , List<int> actorsIds)
        {
            ModelState.Remove("file");
            if (ModelState.IsValid)
            {
                var movie = new Movie();

                if (file != null)
                {
                    string random = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);


                    string fileName = random + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies");


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    movie.ImgUrl = fileName;

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ther is no img");
                    var categories = categoryRepository.Get();
                    var cinemas = cinemaRepository.Get();
                    var actors = actorRepository.Get();
                    ViewBag.cinemas = cinemas;
                    ViewBag.categories = categories;
                    ViewBag.actors = actors;
                    return View(movieVM);
                }

                if (movieVM.EndDate > DateTime.Now && movieVM.EndDate > movieVM.StartDate)
                {
                    if (movie.StartDate > DateTime.Now)
                        movie.MovieStatus = MovieStatus.Upcoming;
                    else if (movie.StartDate <= DateTime.Now && movie.EndDate >= DateTime.Now)
                        movie.MovieStatus = MovieStatus.Available;
                    else
                        movie.MovieStatus = MovieStatus.Expired;

                    movie.Name = movieVM.Name;
                    movie.Description = movieVM.Description;
                    movie.Price = movieVM.Price;
                    movie.TrailerUrl = movieVM.TrailerUrl;
                    movie.StartDate = movieVM.StartDate;
                    movie.EndDate = movieVM.EndDate;
                    movie.CategoryId= movieVM.CategoryId;
                    movie.CinemaId = movieVM.CinemaId;

                }
                else
                {
                    if (movieVM.EndDate <= movieVM.StartDate)
                        ModelState.AddModelError(string.Empty, " the end date can't be before the start date ");
                              if (movieVM.EndDate < DateTime.Now)
                        ModelState.AddModelError(string.Empty, " the end date can't be before today date ");
                    var categories = categoryRepository.Get();
                    var cinemas = cinemaRepository.Get();
                    var actors = actorRepository.Get();
                    ViewBag.cinemas = cinemas;
                    ViewBag.categories = categories;
                    ViewBag.actors = actors;
                    return View(movieVM);
                }

                movieRepository.Create(movie);
                movieRepository.Commit();

                var actormovie = new ActorMovie();
                actormovie.MovieId=movie.Id;
                foreach(var item in actorsIds)
                {
                    actormovie.ActorId = item;
                    actorMovieRepository.Create(actormovie);
                actorMovieRepository.Commit();
                }

            return RedirectToAction("index");
            }
            else
            {

            var categories = categoryRepository.Get();
            var cinemas = cinemaRepository.Get();
            var actors = actorRepository.Get();
            ViewBag.cinemas = cinemas;
            ViewBag.categories = categories;
            ViewBag.actors = actors;
            return View(movieVM);
            }
        }
        public IActionResult Edit(int id)
        {
            var movie = movieRepository.GetOne(expression: e => e.Id == id);
            var movieVM = new MovieVM()
            {
                Name = movie.Name,
                Description = movie.Description,
                StartDate = movie.StartDate,
                EndDate = movie.EndDate,
                Price = movie.Price,
                TrailerUrl = movie.TrailerUrl,

            };
            return View(movieVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie()
                {
                    Name = movieVM.Name,
                    Description = movieVM.Description,
                    StartDate = movieVM.StartDate,
                    EndDate = movieVM.EndDate,
                    Price = movieVM.Price,
                    TrailerUrl = movieVM.TrailerUrl,

                };
                movieRepository.Edit(movie);
                movieRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(movieVM);
        }
    }
}
