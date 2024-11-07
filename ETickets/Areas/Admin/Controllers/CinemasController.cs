using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemasController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;

        public CinemasController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get(includeProps: [e => e.Movies]);
            return View(cinemas);
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(CinemaVM cinemaVM)
        {
            if (ModelState.IsValid)
            {
                var cinema = new Cinema()
                {
                    Name = cinemaVM.Name,
                    Address = cinemaVM.Address,
                    Description = cinemaVM.Description
                };
                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(cinemaVM);
            }
        }
    }
}
