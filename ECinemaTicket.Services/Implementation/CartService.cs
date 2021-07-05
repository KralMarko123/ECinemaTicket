using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Domain.DTO;
using ECinemaTicket.Repository.Interface;
using ECinemaTicket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECinemaTicket.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IRepository<EmailMessage> _mailRepository;




        public CartService(IRepository<Cart> cartRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketInOrderRepository
            , IRepository<EmailMessage> mailRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _mailRepository = mailRepository;
        }

        public bool deleteTicketFromCart(string userId, Guid id)
        {
                        
        if(!string.IsNullOrEmpty(userId) && id != null)
            {
                //deletion code
                var loggedInUser = this._userRepository.Get(userId);

                var cart = loggedInUser.UserCart;

                var ticketToDelete = cart.TicketInCarts
                    .Where(z => z.TicketId.Equals(id))
                    .FirstOrDefault();

                cart.TicketInCarts.Remove(ticketToDelete);

                this._cartRepository.Update(cart);

                return true;
            }
            return false;
        }

        public CartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var cart = loggedInUser.UserCart;

            var allTickets = cart.TicketInCarts.ToList();


            var allTicketsPrice = allTickets.Select(z => new
            {
                price = z.Ticket.MoviePrice,
                quantity = z.Quantity
            }).ToList();

            double total = 0;

            foreach (var item in allTicketsPrice)
            {
                total += item.price * item.quantity;
            }


            CartDto cartDtoItem = new CartDto
            {
                TicketInCarts = allTickets,
                totalPrice = total 
            };

            return cartDtoItem;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {

                var loggedInUser = this._userRepository.Get(userId);

                var cart = loggedInUser.UserCart;

                //MAIL
                EmailMessage message = new EmailMessage()
                {
                    MailTo = loggedInUser.Email,
                    Subject = "Successfully created order",
                    Status = false
                };

              
                Order orderItem = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    User = loggedInUser
                };

                this._orderRepository.Insert(orderItem);

                List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

                var result =  cart.TicketInCarts.Select(z => new TicketInOrder
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderItem.Id,
                        TicketId = z.Ticket.Id,
                        SelectedTicket = z.Ticket,
                        Quantity = z.Quantity,
                        UserOrder = orderItem

                    }).ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order is completed. The order contains: ");

                double totalPrice = 0;
                for(int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    totalPrice += item.Quantity * item.SelectedTicket.MoviePrice;
                    sb.AppendLine(i.ToString() + ". " + item.SelectedTicket.MovieName + " with a price of: " + item.SelectedTicket.MoviePrice + "MKD and a quantity of: " + item.Quantity);
                }
                sb.AppendLine("Your Total Price: " + totalPrice.ToString());

                message.Content = sb.ToString();

                ticketInOrders.AddRange(result);

                foreach (var item in ticketInOrders)
                {
                    this._ticketInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.TicketInCarts.Clear();

                this._mailRepository.Insert(message);

                this._userRepository.Update(loggedInUser);

                return true;
            }
            return false;
        }
    }
}
