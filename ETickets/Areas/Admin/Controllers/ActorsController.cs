using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorsController : Controller
    {
        private readonly IActorRepository actorRepository;
        private readonly IMovieRepository movieRepository;
        private readonly IActorMovieRepository actorMovieRepository;

        public ActorsController(IActorRepository actorRepository , IMovieRepository movieRepository
            ,IActorMovieRepository actorMovieRepository)
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
            ViewBag.movies= movies;
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

               return RedirectToAction("Index", "actors" , new  {area="admin" });

            }
            else
            {
                var movies = movieRepository.Get();
                ViewBag.movies = movies;
                return View(actorVM);
            }
        }
    }
}
