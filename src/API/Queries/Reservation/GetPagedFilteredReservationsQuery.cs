using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using MediatR;

namespace HotelReservation.API.Queries.Reservation
{
    public class GetPagedFilteredReservationsQuery : IRequest<BasePagedResponseModel<ReservationBriefResponseModel>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public ReservationsFilter ReservationsFilter { get; set; }

        public string Route { get; set; }
    }
}
