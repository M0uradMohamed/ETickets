using ETickets.Data;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Controllers
{
    public class SearchController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index(string name)
        {
            var movies = context.Movies.Where(e=>e.Name.Contains(name)).ToList();
            return View(movies);
        }
    }
}
