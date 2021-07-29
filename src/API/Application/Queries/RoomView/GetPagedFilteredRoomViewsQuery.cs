using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Application.Queries.RoomView
{
    public class GetPagedFilteredRoomViewsQuery : IRequest<BasePagedResponseModel<RoomViewResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public RoomViewsFilter RoomViewsFilter { get; set; }
    }
}
