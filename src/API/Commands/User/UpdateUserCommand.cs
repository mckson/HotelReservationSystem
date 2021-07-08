using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.User
{
    public class UpdateUserCommand : IRequest<UserResponseModel>
    {
        [Required]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password mismatch")]
        public string PasswordConfirm { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<string> Roles { get; set; }

        public IEnumerable<string> Hotels { get; set; }
    }
}
