using ETickets.Repository.IRepository;
using ETickets.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.adminRole)]
    public class OrdersController : Controller
    {
        private readonly IPurchaseRepository purchaseRepository;

        public OrdersController(IPurchaseRepository purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
        }
        public IActionResult ManagementPurchases()
        {
            var purchase = purchaseRepository.Get(includeProps: [e=>e.OrderItems]);
            return View(purchase);
        }
    }
}
