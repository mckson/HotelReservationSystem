using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries.User
{
    public class GetUserByIdQuery : IRequest<UserResponseModel>
    {
        public Guid Id { get; set; }
    }
}
