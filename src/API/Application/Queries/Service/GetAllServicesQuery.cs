using System.Collections.Generic;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Queries.Service
{
    public class GetAllServicesQuery : IRequest<IEnumerable<ServiceResponseModel>>
    {
    }
}
