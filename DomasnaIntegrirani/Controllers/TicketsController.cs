using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ECinemaTicket.Services.Interface;
using ECinemaTicket.Domain.DomainModels;
using ECinemaTicket.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.Net.Http;
using System.IO;
using ECinemaTicket.Domain;

namespace DomasnaIntegrirani.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
       
        public IActionResult Index()
        {
            var allTickets = this._ticketService.GetAllTickets();

            return View(allTickets);
        }

        //FILTER TICKETS
        //public IActionResult FilterTickets()
        //{
        //    DateTime today = DateTime.Today;

        //    var filteredTickets = this._ticketService.GetAllTickets().Where(z => z.Date.CompareTo(today) > 0);

        //    return RedirectToAction("Index" , "Tickets", filteredTickets);
        //}




        [Authorize]
        public IActionResult AddTicketToCart(Guid? Id)
        {
            var model = this._ticketService.GetCartInfo(Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTicketToCart([Bind("TicketId", "Quantity")] AddToCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(item);

        }


        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieName,MovieImage,MovieDescription,MoviePrice,MovieRating,Date,MovieCategory")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                this._ticketService.CreateNewTicket(ticket);

                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieName,MovieImage,MovieDescription,MoviePrice,MovieRating,MovieCategory,Date")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._ticketService.DeleteTicket(id);

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return this._ticketService.GetDetailsForTicket(id) != null;
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public FileContentResult ExportAllTickets()
        {
            string fileName = "Tickets.xlsx"; 
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");

                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Movie Name";
                worksheet.Cell(1, 3).Value = "Movie Price";
                worksheet.Cell(1, 4).Value = "Movie Category";
                worksheet.Cell(1, 5).Value = "Movie Rating";

                HttpClient client = new HttpClient();

                string URL = "https://localhost:44355/api/Admin/GetAllTickets";

                HttpResponseMessage response = client.GetAsync(URL).Result;

                var result = response.Content.ReadAsAsync<List<Ticket>>().Result;
                
                for (int i = 1; i <= result.Count; i++)
                {
                    var item = result[i-1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();

                    worksheet.Cell(i + 1, 2).Value = item.MovieName;

                    worksheet.Cell(i + 1, 3).Value = item.MoviePrice.ToString();

                    worksheet.Cell(i + 1, 4).Value = item.MovieCategory;

                    worksheet.Cell(i + 1, 5).Value = item.MovieRating.ToString();

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }


        [HttpGet]
        public IActionResult ExportTicketsByCategory()
        {            
                return View();          
        }

        [HttpPost]
        public IActionResult ExportTicketsByCategory(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string categoryToExport = model.Category;

                string fileName = "TicketsByCategory.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");

                    worksheet.Cell(1, 1).Value = "Ticket Id";
                    worksheet.Cell(1, 2).Value = "Movie Name";
                    worksheet.Cell(1, 3).Value = "Movie Price";
                    worksheet.Cell(1, 4).Value = "Movie Category";
                    worksheet.Cell(1, 5).Value = "Movie Rating";

                    HttpClient client = new HttpClient();

                    string URL = "https://localhost:44355/api/Admin/GetAllTicketsByCategory?categoryName=" + categoryToExport;

                    HttpResponseMessage response = client.GetAsync(URL).Result;

                    var result = response.Content.ReadAsAsync<List<Ticket>>().Result;

                    for (int i = 1; i <= result.Count; i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();

                        worksheet.Cell(i + 1, 2).Value = item.MovieName;

                        worksheet.Cell(i + 1, 3).Value = item.MoviePrice.ToString();

                        worksheet.Cell(i + 1, 4).Value = item.MovieCategory;

                        worksheet.Cell(i + 1, 5).Value = item.MovieRating.ToString();

                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(content, contentType, fileName);
                    }

                }                             
            }
            return View(model);
        }
    }
}
