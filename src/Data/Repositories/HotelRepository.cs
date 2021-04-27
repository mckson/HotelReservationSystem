using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : IRepository<HotelEntity>
    {
        private readonly HotelContext _db;

        public HotelRepository(HotelContext context)
        {
            _db = context;
        }

        public IEnumerable<HotelEntity> GetAll()
        {
            return _db.Hotels
                /*.Include(hotel => hotel.Location)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Guest)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Reservation)
                .Include(hotel => hotel.Guests)*/;
        }

        public async Task<IEnumerable<HotelEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public HotelEntity Get(int id)
        {
            return _db.Hotels
                /*.Include(hotel => hotel.Location)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Guest)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Reservation)
                .Include(hotel => hotel.Guests)*/
                .FirstOrDefault(hotel => hotel.Id == id);
        }

        public async Task<HotelEntity> GetAsync(int id)
        {
            return await Task.Run(() => Get(id));
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return _db.Hotels
                /*.Include(hotel => hotel.Location)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Guest)
                .Include(hotel => hotel.Rooms)
                    .ThenInclude(room => room.Reservation)
                .Include(hotel => hotel.Guests)*/
                .Where(predicate);
        }

        public async Task<IEnumerable<HotelEntity>> FindAsync(Func<HotelEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public void Create(HotelEntity hotel)
        {
            _db.Hotels.Add(hotel);
        }

        public async Task CreateAsync(HotelEntity hotel)
        {
            await _db.Hotels.AddAsync(hotel);
        }

        //change implementation
        public void Update(HotelEntity newHotel)
        {
            var oldHotel = _db.Hotels.Find(newHotel.Id);
            oldHotel = newHotel;
        }

        public async Task UpdateAsync(HotelEntity newItem)
        {
            await Task.Run(() => Update(newItem));
        }

        public void Delete(int id)
        {
            var hotel = _db.Hotels.Find(id);

            if (hotel != null)
                _db.Hotels.Remove(hotel);
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id);

            if (hotel != null)
                _db.Hotels.Remove(hotel);
        }
    }
}