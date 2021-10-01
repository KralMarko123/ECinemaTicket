using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECinemaTicket.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {

        [Required(ErrorMessage = "Name is required.")]
        public string MovieName { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public string MovieImage { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string MovieDescription { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public int MoviePrice { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int MovieRating { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public string MovieCategory { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        public virtual ICollection<TicketInCart> TicketInCarts { get; set; }

        public virtual ICollection<TicketInOrder> Orders { get; set; }


    }
}
