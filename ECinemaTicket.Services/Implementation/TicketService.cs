using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Domain.DTO;
using ECinemaTicket.Domain.Identity;
using ECinemaTicket.Repository.Interface;
using ECinemaTicket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECinemaTicket.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInCart> _ticketInCartRepository;
        private readonly IUserRepository _userRepository;


        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInCart> ticketInCartRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInCartRepository = ticketInCartRepository;
        }

        public bool AddToCart(AddToCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userCart = user.UserCart;

            if (item.TicketId != null && userCart != null)
            {
                var movieTicket = this.GetDetailsForTicket(item.TicketId);

                if (movieTicket != null)
                {
                    TicketInCart ticketToAdd = new TicketInCart
                    {
                        Id = Guid.NewGuid(),
                        Ticket = movieTicket,
                        TicketId = movieTicket.Id,
                        Cart = userCart,
                        CartId = userCart.Id,
                        Quantity = item.Quantity
                    };
            
                        this._ticketInCartRepository.Insert(ticketToAdd);
                        return true;               
                }

                return false;
            }
            return false;
        }

        public void CreateNewTicket(Ticket t)
        {
            this._ticketRepository.Insert(t);
        }

        public void DeleteTicket(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);

            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return this._ticketRepository.GetAll().ToList();
        }

        public AddToCartDto GetCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            AddToCartDto model = new AddToCartDto
            {
                SelectedTicket = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public void UpdateExistingTicket(Ticket t)
        {
            this._ticketRepository.Update(t);
        }
    }
}
