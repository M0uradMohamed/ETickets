namespace ETickets.Models
{
    public class purchase
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public long total {  get; set; }
       public List<OrderItem> OrderItems { get; set; }

    }
}
