namespace ETickets.Models
{
    public class OrderItem
    {
        public int MovieId { get; set; }
        public string ApplicationUserId { get; set; }
        public int Count { get; set; }

        public Movie Movie { get; set; }    
        public ApplicationUser User { get; set; }
        public int? purchaseId { get; set; }
      public purchase purchase { get; set; }

    }
}
