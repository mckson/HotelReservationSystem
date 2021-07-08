using HotelReservation.Data.Entities;
using System;
using System.Collections.Generic;

namespace HotelReservation.API.Interfaces
{
    public interface IUserHelper
    {
        IEnumerable<ReservationEntity> GetUserReservations(string email);

        bool IsCurrentUser(Guid id);
    }
}
