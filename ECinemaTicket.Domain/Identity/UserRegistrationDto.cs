using System.ComponentModel.DataAnnotations;

namespace ECinemaTicket.Domain.Identity
{
    public class UserRegistrationDto
    {
        
        [StringLength(50)]
        public string FirstName { get; set; }
       
        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get;  set; }
    }
}
