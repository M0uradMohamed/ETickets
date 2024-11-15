using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository.IRepository;
using ETickets.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace ETickets.Areas.Customer.Controllers
{
    [Area("Customer")]
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IOrderItemRepository orderItemRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMovieRepository movieRepository;

        public CartController(IOrderItemRepository orderItemRepository, UserManager<IdentityUser> userManager
            , IMovieRepository movieRepository)
        {
            this.orderItemRepository = orderItemRepository;
            this.userManager = userManager;
            this.movieRepository = movieRepository;
        }
        public ActionResult Index()
        {
            var userId = userManager.GetUserId(User);
            var items = orderItemRepository.Get(expression: e => e.ApplicationUserId == userId
            , includeProps: [e => e.Movie], tracked: false);

            var sum = items.Sum(e => e.Movie.Price * e.Count);
            ViewBag.TotalPrice = Math.Round(sum, 2);


            return View(items);
        }

        [HttpPost]
        public IActionResult addToCart(MovieOrderVM movieOrderVM)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole(SD.customerRole))
            {

                if (movieOrderVM.count > 0)
                {
                    var orderitem = new OrderItem();
                    orderitem.ApplicationUserId = userManager.GetUserId(User);
                    orderitem.MovieId = movieOrderVM.Id;
                    orderitem.Count = movieOrderVM.count;
                    orderitem.purchaseId = null;

                    var cart = orderItemRepository.GetOne(expression: e => e.MovieId == movieOrderVM.Id && e.ApplicationUserId == userManager.GetUserId(User), tracked: false);

                    if (cart == null)
                        orderItemRepository.Create(orderitem);
                    else
                        cart.Count += orderitem.Count;

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
        public IActionResult delete(int id)
        {
            var orderItem = orderItemRepository.GetOne(expression: e => e.ApplicationUserId == userManager.GetUserId(User), tracked: false);
            orderItemRepository.Delete(orderItem);
            orderItemRepository.Commit();

            return RedirectToAction("index");
        }
        public IActionResult payment()
        {

            var userId = userManager.GetUserId(User);
            var items = orderItemRepository.Get(expression: e => e.ApplicationUserId == userId
            , includeProps: [e => e.Movie, e => e.User], tracked: false);

            if (items.Count() != 0)
            {



                foreach (var item in items)
                {

                    if (item.Movie.Quantity <= 0)
                    {
                        TempData["QuantityOut"] = $"{item.Movie.Name} out fo stock";
                        return RedirectToAction("index");
                    }

                }

                var sum = items.Sum(e => e.Movie.Price * e.Count);

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/success",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/customer/checkout/cancel",
                };

                foreach (var item in items)
                {
                    options.LineItems.Add(
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Movie.Name
                            },
                            UnitAmount = (long)item.Movie.Price * 100,
                        },
                        Quantity = item.Count,

                    });
                }

                var service = new SessionService();
                var session = service.Create(options);

                return Redirect(session.Url);
            }
            else
            {
                TempData["noItems"] = "the is no items to purchase";
                return RedirectToAction("index");
            }

        }



    }
}
