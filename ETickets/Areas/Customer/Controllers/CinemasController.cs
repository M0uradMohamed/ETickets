using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CinemasController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;

        public CinemasController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }

        public IActionResult Index()
        {
            var categories = cinemaRepository.Get(includeProps: [e => e.Movies]);
            return View(categories);
        }
        public IActionResult Movies(int id)
        {
            var movies = cinemaRepository.GetOne(includeProps: [e => e.Movies], expression: e => e.Id == id);
            return View(movies);
        }
    }
}
