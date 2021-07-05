using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Domain.Identity;
using ECinemaTicket.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomasnaIntegrirani.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ECinemaApplicationUser> userManager;
        private readonly ITicketService _ticketService;

        public AdminController(IOrderService orderService, UserManager<ECinemaApplicationUser> userManager, ITicketService ticketService)
        {
            
            this._orderService = orderService;
            this._ticketService = ticketService;
            this.userManager = userManager;
        }
        //TICKET API ACTIONS
        [HttpGet("[action]")]
        public List<Ticket> GetAllTickets()
        {
            return this._ticketService.GetAllTickets();
        }

        [HttpGet("[action]")]
        public List<Ticket> GetAllTicketsByCategory(string categoryName)
        {
            return this._ticketService.GetAllTickets().Where(z => z.MovieCategory.Equals(categoryName)).ToList();
        }

        [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();
        }

        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderService.GetOrderDetails(model);
        }

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<ECinemaApplicationUser> model)
        {
            bool status = true;

            foreach(var item in model)
            {
                var userCheck =   userManager.FindByEmailAsync(item.Email).Result;

                if(userCheck == null)
                {
                    var user = new ECinemaApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,                   
                        Role = item.Role,
                        UserCart = new Cart()
                    };
                    var result =  userManager.CreateAsync((ECinemaApplicationUser)user, item.Password).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, user.Role);
                    }

                    status = status && result.Succeeded;

                    
                }
                else
                {
                    continue;
                }
            }

            return status;
        }
    }
}
