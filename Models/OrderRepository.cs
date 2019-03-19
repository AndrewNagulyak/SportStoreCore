using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SportStoreCore.Models
{
    public class EfOrderRepository:IOrderRepository
    {
        private ApplicationDbContext dbContext;

        public EfOrderRepository(ApplicationDbContext ctx)
        {
            dbContext = ctx;
        }
        public IQueryable<Order> Orders => dbContext.Orderers
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);
        public void SaveOrder(Order order)
        {
            dbContext.AttachRange(order.Lines.Select(l=>l.Product));
            if (order.OrderID == 0)
            {
                dbContext.Orderers.Add(order);
            }
            dbContext.SaveChanges();
        }
    }
}
    