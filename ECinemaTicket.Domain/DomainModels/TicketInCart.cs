using System;

namespace ECinemaTicket.Domain.DomainModels
{
    public class TicketInCart : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public Guid CartId { get; set; }
        public Cart Cart { get; set; }

        public int Quantity { get; set; }
    }
}
