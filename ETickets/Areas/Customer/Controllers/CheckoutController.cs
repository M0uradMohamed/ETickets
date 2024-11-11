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
            , includeProps: [e => e.Movie], tracked:false);



            var purchase = new purchase();
            purchase.userId= userId;
            purchase.OrderItems = items.ToList();
            var sum = items.Sum(e => e.Movie.Price * e.count);
            ViewBag.TotalPrice = Math.Round(sum, 2);
            purchase.total = (long)sum*100;

      

          /*  foreach (var item in items)
            {
                var movieId = item.Movie.Id;

                var movie = movieRepository.GetOne(expression: e => e.Id == movieId,tracked:false);
                movie.Quantity-=item.count;

                movieRepository.Edit(movie);
                movieRepository.Commit();
            }*/
            var orderitems = orderItemRepository.Get(expression: e => e.ApplicationUserId == userId
, includeProps: [e => e.Movie], tracked: false);

            foreach (var item in orderitems)
            {
                orderItemRepository.Delete(item);
                orderItemRepository.Commit();
            }
            var paur = purchase;

            purchaseRepository.Create(paur);
            purchaseRepository.Commit();

            return View();
        }  
        public IActionResult cancel()
        {
            return View();
        }
    }
}
