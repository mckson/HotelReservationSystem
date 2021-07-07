using Castle.Core.Internal;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HotelReservation.Data.Constants
{
    public static class FilterExpressions
    {
        public static Expression<Func<RoomEntity, bool>> GetRoomFilterExpression(RoomsFilter filter)
        {
            return room =>
                room.HotelId.Value.Equals(filter.HotelId) &&
                ((!filter.DateIn.HasValue || !filter.DateOut.HasValue) || !room.ReservationRooms.Any(rr =>
                    (rr.Reservation.DateIn >= filter.DateIn && rr.Reservation.DateIn < filter.DateOut) ||
                    (rr.Reservation.DateOut > filter.DateIn && rr.Reservation.DateOut <= filter.DateOut)));
        }

        public static Expression<Func<HotelEntity, bool>> GetHotelFilterExpression(HotelsFilter filter)
        {
            return hotel =>
                ((!filter.DateIn.HasValue || !filter.DateOut.HasValue) || hotel.Rooms.Any(room =>
                    !room.ReservationRooms.Any(rr =>
                        (rr.Reservation.DateIn >= filter.DateIn && rr.Reservation.DateIn < filter.DateOut) ||
                        (rr.Reservation.DateOut > filter.DateIn && rr.Reservation.DateOut <= filter.DateOut)))) &&
                (!filter.ManagerId.HasValue || hotel.HotelUsers.Any(hu => hu.UserId == filter.ManagerId.Value)) &&
                (filter.Name.IsNullOrEmpty() || hotel.Name.StartsWith(filter.Name)) &&
                (filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City)) &&
                (filter.Services.IsNullOrEmpty() || hotel.Services.Any(service =>
                    filter.Services.First().IsNullOrEmpty() || service.Name.StartsWith(filter.Services.First())));
        }
    }
}
