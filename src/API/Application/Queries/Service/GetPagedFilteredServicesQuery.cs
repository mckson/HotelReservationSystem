using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Application.Queries.Service
{
    public class GetPagedFilteredServicesQuery : IRequest<BasePagedResponseModel<ServiceResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public ServicesFilter ServicesFilter { get; set; }
    }
}
