using ECinemaTicket.Domain.DomainModels;
using System.Collections.Generic;

namespace ECinemaTicket.Domain.DTO
{
    public class CartDto
    {
        public List<TicketInCart> TicketInCarts { get; set; }

        public double totalPrice { get; set; }
    }
}
