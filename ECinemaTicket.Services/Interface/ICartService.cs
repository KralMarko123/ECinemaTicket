using ECinemaTicket.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinemaTicket.Services.Interface
{
    public interface ICartService
    {
        CartDto getShoppingCartInfo(string userId);

        bool deleteTicketFromCart(string userId, Guid id);

        bool orderNow(string userId);
    }
}
