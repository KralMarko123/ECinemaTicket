
using ECinemaTicket.Domain.Identity;
using System;
using System.Collections.Generic;

namespace ECinemaTicket.Domain.DomainModels
{
    public class Cart : BaseEntity
    {

        public string ownerId { get; set; }

        public ECinemaApplicationUser cartOwner { get; set; }

        public virtual ICollection<TicketInCart> TicketInCarts { get; set; }

    }
}
