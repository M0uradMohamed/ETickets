using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using ETickets.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.adminRole)]
    public class ActorsController : Controller
    {
        private readonly IActorRepository actorRepository;
        private readonly IMovieRepository movieRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public ActorsController(IActorRepository actorRepository, IMovieRepository movieRepository
            , IActorMovieRepository actorMovieRepository)
        {
            this.actorRepository = actorRepository;
            this.movieRepository = movieRepository;
            this.actorMovieRepository = actorMovieRepository;
        }

        public IActionResult Index()
        {
            var actors = actorRepository.Get();
            return View(actors);
        }
        public IActionResult create()
        {
            var movies = movieRepository.Get();
            ViewBag.movies = movies;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(ActorVM actorVM, IFormFile file, List<int> moviesIds)
        {
            ModelState.Remove("file");
            ModelState.Remove("moviesIds");
            if (ModelState.IsValid)
            {
                var actor = new Actor();
                if (file != null)
                {
                    string random = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);


                    string fileName = random + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast");


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    actor.ProfilePicture = fileName;

                }
                else
                {
                    var movies = movieRepository.Get();
                    ViewBag.movies = movies;
                    return View(actorVM);
                }

                actor.FirstName = actorVM.FirstName;
                actor.LastName = actorVM.LastName;
                actor.Bio = actorVM.Bio;
                actor.News = actorVM.News;

                actorRepository.Create(actor);
                actorRepository.Commit();

                var actormovie = new ActorMovie();
                actormovie.ActorId = actor.Id;
                foreach (var item in moviesIds)
                {
                    actormovie.MovieId = item;
                    actorMovieRepository.Create(actormovie);
                    actorMovieRepository.Commit();
                }

                return RedirectToAction("Index", "actors", new { area = "admin" });

            }
            else
            {
                var movies = movieRepository.Get();
                ViewBag.movies = movies;
                return View(actorVM);
            }
        }
        public IActionResult Edit(int id)
        {
            var actor = actorRepository.GetOne(expression: e => e.Id == id, includeProps: [e => e.Movies]);
            if (actor != null)
            {
                var actorVMEdit = new ActorVMEdit()
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    Bio = actor.Bio,
                    News = actor.News,
                    Movies = actor.Movies,
                    ProfilePicture = actor.ProfilePicture,
                };

                var movies = movieRepository.Get();
                ViewBag.movies = movies;
                return View(actorVMEdit);
            }
            else
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ActorVMEdit actorVMEdit, IFormFile file, List<int> moviesIds)
        {
            ModelState.Remove("file");
            ModelState.Remove("moviesIds");
            if (ModelState.IsValid)
            {
                var actor = new Actor();
                if (file != null)
                {

                    string random = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);


                    string fileName = random + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", fileName);

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast");


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    actor.ProfilePicture = fileName;

                }
                else
                {
                    actor.ProfilePicture=actorVMEdit.ProfilePicture;
                }

                actor.Id = actorVMEdit.Id;
                actor.FirstName = actorVMEdit.FirstName;
                actor.LastName = actorVMEdit.LastName;
                actor.Bio=actorVMEdit.Bio;
                actor.News = actorVMEdit.News;


                actorRepository.Edit(actor);
                actorRepository.Commit();


                var actorsmovies = actorMovieRepository.Get(expression: e => e.ActorId == actor.Id, tracked: false);
                foreach (var item in actorsmovies)
                {
                    bool chechmovieStillIn = false;
                    foreach (var movieId in moviesIds)
                    {
                        if (item.MovieId == movieId)
                        {
                            chechmovieStillIn = true;
                            moviesIds.Remove(movieId);
                            break;
                        }
                    }
                    if (chechmovieStillIn == false)
                    {
                        actorMovieRepository.Delete(new() { MovieId = item.MovieId, ActorId = actor.Id });
                        actorMovieRepository.Commit();
                    }
                }
                var actorMovie = new ActorMovie() { ActorId = actor.Id };
                foreach (var movieId in moviesIds)
                {
                    actorMovie.MovieId = movieId;
                    actorMovieRepository.Create(actorMovie);
                    actorMovieRepository.Commit();
                }
                return RedirectToAction("index");

            }
            else
            {
                var movies = movieRepository.Get();
                ViewBag.movies = movies;
                return View(actorVMEdit);
            }

        }
        public IActionResult Delete(int id)
        {
            var actor = actorRepository.GetOne(expression: e => e.Id == id);
            if (actor != null)
            {

                var actormovies = actorMovieRepository.Get(expression: e => e.ActorId == actor.Id, tracked: false);
                foreach (var actorMovie in actormovies)
                {
                    actorMovieRepository.Delete(actorMovie);
                    actorMovieRepository.Commit();
                }
                actorRepository.Delete(actor);
                actorRepository.Commit();
                return RedirectToAction("index");
            }
            return RedirectToAction("notfound", "home", new { area = "customer" });
        }

    }

}
