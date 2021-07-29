﻿using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ReservationRepository : BaseRepository<ReservationEntity>, IReservationRepository
    {
        public ReservationRepository(HotelContext context, ISortHelper<ReservationEntity> sortHelper)
            : base(context, sortHelper)
        {
        }
    }
}