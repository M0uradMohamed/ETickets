using ETickets.Areas.Admin.ViewModels;
using ETickets.Models;
using ETickets.Repository;
using ETickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace ETickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemasController : Controller
    {
        private readonly ICinemaRepository cinemaRepository;

        public CinemasController(ICinemaRepository cinemaRepository)
        {
            this.cinemaRepository = cinemaRepository;
        }
        public IActionResult Index()
        {
            var cinemas = cinemaRepository.Get(includeProps: [e => e.Movies]);
            return View(cinemas);
        }
        public IActionResult create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(CinemaVM cinemaVM, IFormFile file)
        {
            ModelState.Remove(nameof(file));
            if (ModelState.IsValid)
            {
                var cinema = new Cinema();
                if (file != null)
                {
                    string random = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);


                    string fileName = random + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemasLogo", fileName);

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemasLogo");


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    cinema.CinemaLogo = fileName;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "a logo image is required");
                    return View(cinemaVM);
                }



                cinema.Name = cinemaVM.Name;
                cinema.Address = cinemaVM.Address;
                cinema.Description = cinemaVM.Description;

                cinemaRepository.Create(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(cinemaVM);
            }
        }

        public IActionResult Edit(int id)
        {
            var cinema = cinemaRepository.GetOne(expression: e => e.Id == id);
            if (cinema != null)
            {
                var cinemaVMEdit = new CinemaVMEdit()
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    Description = cinema.Description,
                    CinemaLogo = cinema.CinemaLogo

                };
                return View(cinemaVMEdit);
            }
            else
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CinemaVMEdit cinemaVMEdit, IFormFile file)
        {
            ModelState.Remove(nameof(file));
            if (ModelState.IsValid)
            {
                var cinema = new Cinema();
                if (file != null)
                {
                    string random = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);


                    string fileName = random + extension;
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemasLogo", fileName);

                    string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemasLogo");


                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    cinema.CinemaLogo = fileName;
                }
                else
                {
                    cinema.CinemaLogo=cinemaVMEdit.CinemaLogo;
                }


                cinema.Id = cinemaVMEdit.Id;
                cinema.Name = cinemaVMEdit.Name;
                cinema.Address = cinemaVMEdit.Address;
                cinema.Description = cinemaVMEdit.Description;
                

                cinemaRepository.Edit(cinema);
                cinemaRepository.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(cinemaVMEdit);
            }
        }




    }
}
