using ETickets.Models;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="name must be more than 3 letters")]
        public string Name { get; set; }

    }
}
