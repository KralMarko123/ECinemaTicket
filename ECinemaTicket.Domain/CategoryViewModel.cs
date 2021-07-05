using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECinemaTicket.Domain
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]

        public string Category { get; set; }
    }
}
