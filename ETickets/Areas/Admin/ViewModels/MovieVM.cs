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
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
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
