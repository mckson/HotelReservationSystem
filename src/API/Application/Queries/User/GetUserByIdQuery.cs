using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.User
{
    public class GetUserByIdQuery : IRequest<UserResponseModel>
    {
        public Guid Id { get; set; }
    }
}
