using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.Service
{
    public class DeleteServiceCommand : IRequest<ServiceResponseModel>
    {
        public Guid Id { get; set; }
    }
}
