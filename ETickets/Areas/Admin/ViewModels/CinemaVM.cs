using System.ComponentModel.DataAnnotations;

namespace ETickets.Areas.Admin.ViewModels
{
    public class CinemaVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage =" Name must be more than 3 letters")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = " Description must be more than 3 letters")]
        public string Description { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = " Address must be more than 3 letters")]
        public string Address { get; set; }
    }
}
