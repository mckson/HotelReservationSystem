using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Commands.Service
{
    public class UpdateServiceCommand : IRequest<ServiceResponseModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string HotelId { get; set; }
    }
}
