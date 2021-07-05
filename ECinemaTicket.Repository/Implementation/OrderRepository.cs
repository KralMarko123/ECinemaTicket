using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinemaTicket.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.Tickets)
                .Include(z => z.User)
                .Include("Tickets.SelectedTicket")
                .ToListAsync().Result;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.Tickets)
               .Include(z => z.User)
               .Include("Tickets.SelectedTicket")
               .SingleOrDefaultAsync(z => z.Id.Equals(model.Id)).Result;
        }
    }
}
