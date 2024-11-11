using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository.IRepository;
using ETickets.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IOrderItemRepository orderItemRepository;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(IOrderItemRepository orderItemRepository, UserManager<IdentityUser> userManager
            ,IMovieRepository movieRepository)
        {
            this.orderItemRepository = orderItemRepository;
            this.userManager = userManager;
        }
        public ActionResult Index()
        {
            var userId = userManager.GetUserId(User);
            var items = orderItemRepository.Get(expression: e => e.ApplicationUserId == userId
            , includeProps: [e=>e.Movie,e=>e.User]);

                var sum =items.Sum(e => e.Movie.Price * e.count);
            ViewBag.TotalPrice = Math.Round(sum, 2); 
            

            return View(items);
        }

        [HttpPost]
        public IActionResult addToCart(MovieOrderVM movieOrderVM)
        {
            if ( User.Identity.IsAuthenticated && User.IsInRole(SD.customerRole) )
            {

                if (movieOrderVM.count > 0)
                {
                    var orderitem = new OrderItem();
                    orderitem.ApplicationUserId = userManager.GetUserId(User);
                    orderitem.MovieId = movieOrderVM.Id;
                    orderitem.count = movieOrderVM.count;

                    var cart = orderItemRepository.GetOne(expression: e => e.MovieId == movieOrderVM.Id && e.ApplicationUserId == userManager.GetUserId(User));

                    if (cart == null)
                        orderItemRepository.Create(orderitem);
                    else
                        cart.count += orderitem.count;

                    orderItemRepository.Commit();

                    return RedirectToAction("index", "home", new { area = "customer" });
                }
                else
                {
                    TempData["failed"] = "number of ticket must be greater than 0";
                    return RedirectToAction("Details", "home", new { area = "customer", id = movieOrderVM.Id });
                }
            }
            else
            {
                if (User.IsInRole(SD.adminRole))
                {

                    TempData["failed"] = "you can't buy tickets as an admin";
                    return RedirectToAction("Details", "home", new { area = "customer", id = movieOrderVM.Id });
                }
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

        }
    }
}
