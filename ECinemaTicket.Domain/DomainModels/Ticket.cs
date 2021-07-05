using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECinemaTicket.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {

        [Required]
        public string MovieName { get; set; }

        [Required]
        public string MovieImage { get; set; }

        [Required]
        public string MovieDescription { get; set; }

        [Required]
        public int MoviePrice { get; set; }

        [Required]
        public int MovieRating { get; set; }

        [Required]
        public string MovieCategory { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        public virtual ICollection<TicketInCart> TicketInCarts { get; set; }

        public virtual ICollection<TicketInOrder> Orders { get; set; }


    }
}
