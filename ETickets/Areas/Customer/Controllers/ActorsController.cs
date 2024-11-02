using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ActorsController : Controller
    {
        private readonly IActorRepository actorRepository;

        public ActorsController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }

        public IActionResult Index(string actorname, int Id)
        {

            var actor = actorRepository.Get(expression: e => e.Id == Id)
                 .Select(e => new { FullName = e.FirstName + " " + e.LastName, e.Bio, e.Id, e.ProfilePicture, e.News, e.Movies }).FirstOrDefault();

            return View(actor);
        }
        public IActionResult Movies(int id)
        {
            var actor = actorRepository.GetOne(includeProps: [e => e.Movies], expression: e => e.Id == id);

            return View(actor);
        }
    }
}
