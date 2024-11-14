namespace ETickets.Models
{
    public class purchase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public long Total {  get; set; }
       public List<OrderItem> OrderItems { get; set; }

    }
}
