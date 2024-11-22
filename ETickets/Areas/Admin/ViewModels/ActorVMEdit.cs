using ETickets.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Areas.Admin.ViewModels
{
    public class ActorVMEdit
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [MinLength(3)]
        public string Bio { get; set; }

        public string ProfilePicture { get; set; }
        [Required]
        [MinLength(3)]
        public string News { get; set; }
        [ValidateNever]
        public List<Movie> Movies { get; set; }
    }
}
