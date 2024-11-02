using ETickets.Data;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETickets.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.Get(includeProps:[ e=>e.Movies]);
            return View(categories);
        }
        public IActionResult Movies(int id)
        {
            var movies = categoryRepository.GetOne(includeProps: [e => e.Movies], expression:e=>e.Id==id);
            return View(movies);
        }
    }
}
