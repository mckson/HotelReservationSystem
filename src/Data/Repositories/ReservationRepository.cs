using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ReservationRepository : IRepository<ReservationEntity>
    {
        private readonly HotelContext _db;

        public ReservationRepository(HotelContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<ReservationEntity>> FindAsync(Func<ReservationEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public void Create(ReservationEntity reservation)
        {
            _db.Reservations.Add(reservation);
        }

        public async Task CreateAsync(ReservationEntity reservation)
        {
            await _db.Reservations.AddAsync(reservation);
        }

        public async Task UpdateAsync(ReservationEntity newReservation)
        {
            var oldReservation = await _db.Reservations.FindAsync(newReservation.Id);
            oldReservation = newReservation;
        }

        public void Delete(int id)
        {
            var reservation = _db.Reservations.Find(id);

            if (reservation != null)
                _db.Reservations.Remove(reservation);
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);

            if (reservation != null)
                _db.Reservations.Remove(reservation);
        }

        public async Task<ReservationEntity> GetAsync(int id)
        {
            return await _db.Reservations.FindAsync(id);
        }

        public IEnumerable<ReservationEntity> Find(Func<ReservationEntity, bool> predicate)
        {
            return _db.Reservations.Where(predicate);
        }

        public async Task<IEnumerable<ReservationEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public ReservationEntity Get(int id)
        {
            return _db.Reservations.Find(id);
        }

        public IEnumerable<ReservationEntity> GetAll()
        {
            return _db.Reservations;
        }

        public void Update(ReservationEntity newReservation)
        {
            var oldReservation = _db.Reservations.Find(newReservation.Id);
            oldReservation = newReservation;
        }
    }
}