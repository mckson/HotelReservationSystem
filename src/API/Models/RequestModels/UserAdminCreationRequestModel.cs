using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class UserAdminCreationRequestModel
    {
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "One or more role is required")]
        public IEnumerable<string> Roles { get; set; }
    }
}