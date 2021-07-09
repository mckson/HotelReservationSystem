using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Commands.User
{
    public class CreateUserCommand : IRequest<UserResponseModel>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
