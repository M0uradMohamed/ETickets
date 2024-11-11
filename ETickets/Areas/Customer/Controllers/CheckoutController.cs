using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Customer.Controllers
{
    
    [Area("Customer")]
    [AllowAnonymous]
    public class CheckoutController : Controller
    {
        private readonly IOrderItemRepository orderItemRepository;
        private readonly UserManager<IdentityUser> userManager;

        public CheckoutController(IOrderItemRepository orderItemRepository, UserManager<IdentityUser> userManager
            , IMovieRepository movieRepository)
        {
            this.orderItemRepository = orderItemRepository;
            this.userManager = userManager;
        }
        public IActionResult success()
        {


            return View();
        }  
        public IActionResult cancel()
        {
            return View();
        }
    }
}
