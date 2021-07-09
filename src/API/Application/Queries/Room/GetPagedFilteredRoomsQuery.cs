using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Application.Queries.Room
{
    public class GetPagedFilteredRoomsQuery : IRequest<BasePagedResponseModel<RoomResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public RoomsFilter RoomsFilter { get; set; }

        public string Route { get; set; }
    }
}
