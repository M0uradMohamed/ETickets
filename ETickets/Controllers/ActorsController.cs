using ETickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class ActorsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index(string actorname , int Id)
        {
        
           var actor = context.Actors.Where(e=>e.Id == Id)
                .Select(e=> new { FullName = e.FirstName + " " + e.LastName , e.Bio , e.Id , e.ProfilePicture , e.News , e.Movies }).FirstOrDefault();
        
            return View(actor);
        }
        public IActionResult Movies(int id)
        {
            var actor = context.Actors.Include(e=>e.Movies).Where(e=>e.Id == id).FirstOrDefault();

            return View(actor);
        }
    }
}
