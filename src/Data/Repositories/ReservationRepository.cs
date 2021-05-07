using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ReservationRepository : IRepository<ReservationEntity>
    {
        private readonly HotelContext _db;
        private readonly DbSet<ReservationEntity> _reservations;

        public ReservationRepository(HotelContext context)
        {
            _db = context;
            _reservations = context.Reservations;
        }

        public IEnumerable<ReservationEntity> GetAll() => _reservations;

        public async Task<ReservationEntity> GetAsync(int id) =>
            await _reservations.FirstOrDefaultAsync(reservation => reservation.Id == id);

        public IEnumerable<ReservationEntity> Find(Func<ReservationEntity, bool> predicate) =>
            _reservations.Where(predicate);

        public async Task<ReservationEntity> CreateAsync(ReservationEntity reservation)
        {
            var addedReservationEntry = await _reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();

            return addedReservationEntry.Entity;
        }

        public async Task<ReservationEntity> UpdateAsync(ReservationEntity newReservation)
        {
            var addedReservationEntry = _reservations.Update(newReservation);
            await _db.SaveChangesAsync();

            return addedReservationEntry.Entity;
        }

        public async Task<ReservationEntity> DeleteAsync(int id)
        {
            var reservation = await _reservations.FindAsync(id);

            if (reservation != null)
            {
                _reservations.Remove(reservation);
                await _db.SaveChangesAsync();
            }

            return reservation;
        }
    }
}