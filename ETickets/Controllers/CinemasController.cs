using ETickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CinemasController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var categories = context.Cinemas.Include(e => e.Movies).ToList();
            return View(categories);
        }
        public IActionResult Movies(int id)
        {
            var movies = context.Cinemas.Include(e => e.Movies).Where(e => e.Id == id).FirstOrDefault();
            return View(movies);
        }
    }
}
