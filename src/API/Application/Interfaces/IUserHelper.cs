using System;
using System.Collections.Generic;
using HotelReservation.Data.Entities;

namespace HotelReservation.API.Application.Interfaces
{
    public interface IUserHelper
    {
        IEnumerable<ReservationEntity> GetUserReservations(string email);

        bool IsCurrentUser(Guid id);
    }
}
