using ECinemaTicket.Domain.DomainModels;
using System;

namespace ECinemaTicket.Domain.DTO
{
    public class AddToCartDto
    {
        public Guid TicketId { get; set; }

        public Ticket SelectedTicket { get; set; }

        public int Quantity { get; set; }
    }
}
