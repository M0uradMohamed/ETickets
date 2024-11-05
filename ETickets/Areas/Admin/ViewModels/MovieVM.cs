using ETickets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Areas.Admin.ViewModels
{
    public class MovieVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        public string Description { get; set; }
        [Required]
        [Range(1,1000,ErrorMessage ="price must be greater than 0")]
        public double Price { get; set; }
 
        [Required]
        [Display(Name="Trailer url")]
        public string TrailerUrl { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Cinema name")]
        public int CinemaId { get; set; }
        [Required]
        [Display(Name = "Category name")]
        public int CategoryId { get; set; }
    }
}
