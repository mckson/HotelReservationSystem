using HotelReservation.Business.Models;
using HotelReservation.Data.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IReservationsService : IBaseService<ReservationModel>
    {
        IEnumerable<ReservationModel> GetAllReservations();

        IEnumerable<ReservationModel> GetReservationsByEmail(string email);

        Task<int> GetReservationsCountAsync(ReservationsFilter reservationsFilter);

        IEnumerable<ReservationModel> GetPagedReservations(PaginationFilter paginationFilter, ReservationsFilter reservationsFilter);
    }
}
