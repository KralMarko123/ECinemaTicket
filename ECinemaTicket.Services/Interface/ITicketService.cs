using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Domain.DTO;
using System;
using System.Collections.Generic;

namespace ECinemaTicket.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();

        Ticket GetDetailsForTicket(Guid? id);

        void CreateNewTicket(Ticket t);

        void UpdateExistingTicket(Ticket t);

        AddToCartDto GetCartInfo(Guid? id);

        void DeleteTicket(Guid? id);

        bool AddToCart(AddToCartDto item, string userID);
    }
}
