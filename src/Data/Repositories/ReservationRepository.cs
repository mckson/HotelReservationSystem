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

        public async Task<IEnumerable<ReservationEntity>> GetAllAsync() => await Task.Run(GetAll);

        public ReservationEntity Get(int id) => _reservations.FirstOrDefault(reservation => reservation.Id == id);

        public async Task<ReservationEntity> GetAsync(int id) => await Task.Run(() => Get(id));

        public IEnumerable<ReservationEntity> Find(Func<ReservationEntity, bool> predicate) => _reservations.Where(predicate);

        public async Task<IEnumerable<ReservationEntity>> FindAsync(Func<ReservationEntity, bool> predicate) => await Task.Run(() => Find(predicate));

        public void Create(ReservationEntity reservation)
        {
            _reservations.Add(reservation);
            _db.SaveChanges();
        }

        public async Task CreateAsync(ReservationEntity reservation)
        {
            await _reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }

        //change implementation
        public void Update(ReservationEntity newReservation)
        {
            var oldReservation = _reservations.Find(newReservation.Id);
            oldReservation = newReservation;
            _db.SaveChanges();
        }

        public async Task UpdateAsync(ReservationEntity newReservation)
        {
            var oldReservation = await _reservations.FindAsync(newReservation.Id);
            oldReservation = newReservation;
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var reservation = _reservations.Find(id);

            if (reservation == null) return;

            _reservations.Remove(reservation);
            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _reservations.FindAsync(id);

            if (reservation != null)
            {
                _reservations.Remove(reservation);
                await _db.SaveChangesAsync();
            }
        }
    }
}