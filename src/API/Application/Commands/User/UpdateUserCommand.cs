using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Commands.User
{
    public class UpdateUserCommand : IRequest<UserResponseModel>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string PasswordConfirm { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsRegistered { get; set; }

        public List<string> Roles { get; set; }

        public IEnumerable<Guid> Hotels { get; set; }
    }
}
