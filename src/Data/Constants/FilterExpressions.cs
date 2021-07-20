﻿using Castle.Core.Internal;
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
                room.HotelId.Value.Equals(filter.HotelId) && (!room.UnlockTime.HasValue ||
                                                              room.UnlockTime.Value <= DateTime.UtcNow ||
                                                              (room.UnlockTime.Value > DateTime.UtcNow &&
                                                               (room.LockedByUserId.HasValue &&
                                                                filter.UserId.HasValue &&
                                                                room.LockedByUserId.Value
                                                                    .Equals(filter.UserId.Value)))) &&
                ((!filter.DateIn.HasValue || !filter.DateOut.HasValue) || !room.ReservationRooms.Any(rr =>
                    (rr.Reservation.DateIn >= filter.DateIn && rr.Reservation.DateIn < filter.DateOut) ||
                    (rr.Reservation.DateOut > filter.DateIn && rr.Reservation.DateOut <= filter.DateOut))) &&
                (filter.Name.IsNullOrEmpty() || room.Name.StartsWith(filter.Name)) &&
                (!filter.Number.HasValue || room.RoomNumber.ToString().StartsWith(filter.Number.Value.ToString())) &&
                (!filter.MinFloorNumber.HasValue || room.FloorNumber >= filter.MinFloorNumber.Value) &&
                (!filter.MaxFloorNumber.HasValue || room.FloorNumber <= filter.MaxFloorNumber.Value) &&
                (!filter.MinCapacity.HasValue || room.Capacity >= filter.MinCapacity.Value) &&
                (!filter.MaxCapacity.HasValue || room.Capacity <= filter.MaxCapacity.Value) &&
                (!filter.MinArea.HasValue || room.Area >= filter.MinArea.Value) &&
                (!filter.MaxArea.HasValue || room.Area <= filter.MaxArea.Value) &&
                (!filter.MinPrice.HasValue || room.Price >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || room.Price <= filter.MaxPrice.Value);
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
                (filter.City.IsNullOrEmpty() || hotel.Location.City.StartsWith(filter.City));
        }

        public static Expression<Func<ReservationEntity, bool>> GetReservationFilterExpression(ReservationsFilter reservationsFilter)
        {
            return reservation => reservationsFilter.Email.IsNullOrEmpty() ||
                                  reservation.Email == reservationsFilter.Email;
        }

        public static Expression<Func<UserEntity, bool>> GetUserFilterExpression(UsersFilter usersFilter)
        {
            return user => (usersFilter.Email.IsNullOrEmpty() || user.Email.StartsWith(usersFilter.Email)) &&
                           (usersFilter.FirstName.IsNullOrEmpty() ||
                            user.FirstName.StartsWith(usersFilter.FirstName)) &&
                           (usersFilter.LastName.IsNullOrEmpty() || user.LastName.StartsWith(usersFilter.LastName));
        }
    }
}
