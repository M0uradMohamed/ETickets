namespace ETickets.Models
{
    public class OrderItem
    {
        public int MovieId { get; set; }
        public string ApplicationUserId { get; set; }
        public int count { get; set; }

        public Movie Movie { get; set; }    
        public ApplicationUser User { get; set; }

    }
}
