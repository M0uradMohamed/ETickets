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
                    ModelState.AddModelError(string.Empty, "the ticket count can't be less than 1");
                    return RedirectToAction("Details", "home", new { area = "customer", id = movieOrderVM.Id });
                }
            }
            else
            {
                if (User.IsInRole(SD.adminRole))
                {
                  
                   ModelState.AddModelError(string.Empty, "you are admin , you can't buy ticket");

                    return RedirectToAction("NotFound", "Home", new { area = "Customer" });
                }
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

        }
    }
}
