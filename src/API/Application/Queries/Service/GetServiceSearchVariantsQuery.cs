using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Queries.Service
{
    public class GetServiceSearchVariantsQuery : IRequest<IEnumerable<ServicePromptResponseModel>>
    {
        public ServicesFilter ServicesFilter { get; set; }
    }
}
