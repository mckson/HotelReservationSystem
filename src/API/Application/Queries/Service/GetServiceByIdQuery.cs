using System;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Service
{
    public class GetServiceByIdQuery : IRequest<ServiceResponseModel>
    {
        public Guid Id { get; set; }
    }
}
