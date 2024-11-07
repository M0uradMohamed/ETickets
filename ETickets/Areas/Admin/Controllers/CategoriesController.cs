using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.Get(includeProps: [e => e.Movies]);
            return View(categories);
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var category = new Category() { Name = categoryVM.Name };
                categoryRepository.Create(category);
                categoryRepository.Commit();

                return RedirectToAction("Index", "categories", new { area = "admin" });

            }
            else
            {
                return View(categoryVM);
            }

        }
        public IActionResult Edit(int id)
        {
            var category = categoryRepository.GetOne(expression:e=>e.Id == id);
            if (category != null)
            {
                var categoryVM = new CategoryVM() {  Name = category.Name };
                return View(categoryVM);
            }
            else
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if(ModelState.IsValid)
            {
                var category = new Category()
                {
                    Id = categoryVM.Id,
                    Name = categoryVM.Name
                };
                categoryRepository.Edit(category);
                categoryRepository.Commit();

                return RedirectToAction("Index", "categories", new { area = "admin" });

            }
            else
                return View(categoryVM);

        }
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetOne(expression: e => e.Id == id);
            if (category != null)
            {
              categoryRepository.Delete(category);
                categoryRepository.Commit();

                return RedirectToAction("index");
            }
            else
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });

        }
    }
}
