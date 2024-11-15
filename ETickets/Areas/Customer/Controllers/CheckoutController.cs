using ETickets.Models;
using ETickets.Repository;
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
        private readonly IMovieRepository movieRepository;
        private readonly IPurchaseRepository purchaseRepository;

        public CheckoutController(IOrderItemRepository orderItemRepository, UserManager<IdentityUser> userManager
            , IMovieRepository movieRepository,IPurchaseRepository purchaseRepository)
        {
            this.orderItemRepository = orderItemRepository;
            this.userManager = userManager;
            this.movieRepository = movieRepository;
            this.purchaseRepository = purchaseRepository;
        }
        public IActionResult success()
        {
            var userId = userManager.GetUserId(User);
            var items = orderItemRepository.Get(expression: e => e.ApplicationUserId == userId
            , includeProps: [e => e.Movie]);



            var purchase = new purchase();
            purchase.UserId = userId;
            purchase.Username = userManager.GetUserName(User);
            purchase.OrderItems = items.ToList();
            var sum = items.Sum(e => e.Movie.Price * e.Count);
            ViewBag.TotalPrice = Math.Round(sum, 2);
            purchase.Total = (long)sum * 100;



            foreach (var item in items)
            {
 
              item.Movie.Quantity -= item.Count;

                movieRepository.Edit(item.Movie);
                movieRepository.Commit();
            }
             purchaseRepository.Create(purchase);
             purchaseRepository.Commit();

            foreach (var item in items)
            {
                var order= new OrderItem();
                order = item;
                orderItemRepository.Delete(order);
                orderItemRepository.Commit();
            }



            return View();
        }  
        public IActionResult cancel()
        {
            return View();
        }
    }
}
