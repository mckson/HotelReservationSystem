using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Application.Queries.Hotel
{
    public class GetPagedFilteredHotelsQuery : IRequest<BasePagedResponseModel<HotelResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public HotelsFilter HotelsFilter { get; set; }

        public string Route { get; set; }
    }
}
