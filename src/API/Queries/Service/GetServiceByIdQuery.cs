using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;

namespace HotelReservation.API.Queries.Service
{
    public class GetServiceByIdQuery : IRequest<ServiceResponseModel>
    {
        public Guid Id { get; set; }
    }
}
