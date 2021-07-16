using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.Service
{
    public class DeleteServiceCommand : IRequest<ServiceResponseModel>
    {
        public Guid Id { get; set; }
    }
}
