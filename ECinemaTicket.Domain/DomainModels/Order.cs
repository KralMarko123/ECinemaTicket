using ECinemaTicket.Domain.Identity;
using System;
using System.Collections.Generic;

namespace ECinemaTicket.Domain.DomainModels
{
    public class Order : BaseEntity
    {

        public string UserId { get; set; }

        public ECinemaApplicationUser User { get; set; }

        public virtual ICollection<TicketInOrder> Tickets { get; set; }
    }
}
