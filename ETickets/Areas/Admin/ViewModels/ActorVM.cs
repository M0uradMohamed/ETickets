using System.ComponentModel.DataAnnotations;

namespace ETickets.Areas.Admin.ViewModels
{
    public class ActorVM
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
        [Required]
        [MinLength(3)]
        public string News { get; set; }
    }
}
