using ETickets.Data;
using ETickets.Models;
using ETickets.Repository.IRepository;

namespace ETickets.Repository
{
    public class PurchaseRepository : Repository<purchase>, IPurchaseRepository
    {
        private readonly ApplicationDbContext dbContext;

        public PurchaseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
