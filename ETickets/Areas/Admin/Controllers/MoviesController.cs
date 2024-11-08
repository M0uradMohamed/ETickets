using ETickets.Repository.IRepository;
using ETickets.Models;
using Microsoft.AspNetCore.Mvc;
using ETickets.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using ETickets.Repository;
using ETickets.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.adminRole)]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly ICinemaRepository cinemaRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IActorRepository actorRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public MoviesController(IMovieRepository movieRepository, ICinemaRepository cinemaRepository
            , ICategoryRepository categoryRepository, IActorRepository actorRepository
            , IActorMovieRepository actorMovieRepository)
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
        public IActionResult create(MovieVM movieVM, IFormFile file, List<int> actorsIds)
        {
            ModelState.Remove("file");
            ModelState.Remove("actorsIds");
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
                    if (movieVM.StartDate > DateTime.Now)
                        movie.MovieStatus = MovieStatus.Upcoming;
                    else if (movieVM.StartDate <= DateTime.Now && movieVM.EndDate >= DateTime.Now)
                        movie.MovieStatus = MovieStatus.Available;
                    else
                        movie.MovieStatus = MovieStatus.Expired;

                    movie.Name = movieVM.Name;
                    movie.Description = movieVM.Description;
                    movie.Price = movieVM.Price;
                    movie.TrailerUrl = movieVM.TrailerUrl;
                    movie.StartDate = movieVM.StartDate;
                    movie.EndDate = movieVM.EndDate;
                    movie.CategoryId = movieVM.CategoryId;
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
                actormovie.MovieId = movie.Id;
                foreach (var item in actorsIds)
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
            var movie = movieRepository.GetOne(expression: e => e.Id == id, includeProps: [e => e.Actors, e => e.Category, e => e.Cinema]);
            if (movie != null)
            {

                var movieVMEdit = new MovieVMEdit()
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Description = movie.Description,
                    StartDate = movie.StartDate,
                    EndDate = movie.EndDate,
                    Price = movie.Price,
                    TrailerUrl = movie.TrailerUrl,
                    ImgUrl = movie.ImgUrl,
                    CategoryId = movie.CategoryId,
                    CinemaId = movie.CinemaId,
                    Actors = movie.Actors,

                };
                var categories = categoryRepository.Get();
                var cinemas = cinemaRepository.Get();
                var actors = actorRepository.Get();
                ViewBag.cinemas = cinemas;
                ViewBag.categories = categories;
                ViewBag.actors = actors;
                return View(movieVMEdit);
            }
            return RedirectToAction("NotFound", "Home", new { area = "Customer" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieVMEdit movieVMEdit, IFormFile file, List<int> actorsIds)
        {
            ModelState.Remove("file");
            ModelState.Remove("actorsIds");
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
                    movie.ImgUrl = movieVMEdit.ImgUrl;
                }

                if (movieVMEdit.EndDate > movieVMEdit.StartDate)
                {
                    if (movie.StartDate > DateTime.Now)
                        movie.MovieStatus = MovieStatus.Upcoming;
                    else if (movie.StartDate <= DateTime.Now && movie.EndDate >= DateTime.Now)
                        movie.MovieStatus = MovieStatus.Available;
                    else
                        movie.MovieStatus = MovieStatus.Expired;

                    movie.Id = movieVMEdit.Id;
                    movie.Name = movieVMEdit.Name;
                    movie.Description = movieVMEdit.Description;
                    movie.Price = movieVMEdit.Price;
                    movie.TrailerUrl = movieVMEdit.TrailerUrl;
                    movie.StartDate = movieVMEdit.StartDate;
                    movie.EndDate = movieVMEdit.EndDate;
                    movie.CategoryId = movieVMEdit.CategoryId;
                    movie.CinemaId = movieVMEdit.CinemaId;

                }
                else
                {
                    if (movieVMEdit.EndDate <= movieVMEdit.StartDate)
                        ModelState.AddModelError(string.Empty, " the end date can't be before the start date ");
                    if (movieVMEdit.EndDate < DateTime.Now)
                        ModelState.AddModelError(string.Empty, " the end date can't be before today date ");
                    var categories = categoryRepository.Get();
                    var cinemas = cinemaRepository.Get();
                    var actors = actorRepository.Get();
                    ViewBag.cinemas = cinemas;
                    ViewBag.categories = categories;
                    ViewBag.actors = actors;

                    return View(movieVMEdit);
                }

                movieRepository.Edit(movie);
                movieRepository.Commit();

                var actorsmovies = actorMovieRepository.Get(expression: e => e.MovieId == movie.Id, tracked: false);
                foreach (var item in actorsmovies)
                {
                    bool chechActorStillIn = false;
                    foreach (var actorId in actorsIds)
                    {
                        if (item.ActorId == actorId)
                        {
                            chechActorStillIn = true;
                            actorsIds.Remove(actorId);
                            break;
                        }
                    }
                    if (chechActorStillIn == false)
                    {
                        actorMovieRepository.Delete(new() { ActorId = item.ActorId, MovieId = movie.Id });
                        actorMovieRepository.Commit();
                    }
                }
                var actorMovie = new ActorMovie() { MovieId = movie.Id };
                foreach (var actorId in actorsIds)
                {
                    actorMovie.ActorId = actorId;
                    actorMovieRepository.Create(actorMovie);
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
                return View(movieVMEdit);
            }
        }
        public IActionResult Delete(int id)
        {
            var movie = movieRepository.GetOne(expression: e => e.Id == id);
            if (movie != null)
            {

                var actormovies = actorMovieRepository.Get(expression: e => e.MovieId == movie.Id, tracked: false);
                foreach (var actorMovie in actormovies)
                {
                    actorMovieRepository.Delete(actorMovie);
                    actorMovieRepository.Commit();
                }
                movieRepository.Delete(movie);
                movieRepository.Commit();
                return RedirectToAction("index");
            }
            return RedirectToAction("notfound", "home", new { area = "customer" });
        }
    }
}
