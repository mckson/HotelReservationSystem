using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelContext _db;
        private readonly DbSet<HotelEntity> _hotels;

        public HotelRepository(HotelContext context)
        {
            _db = context;
            _hotels = context.Hotels;
        }

        public IEnumerable<HotelEntity> GetAll(bool asNoTracking = false)
        {
            return asNoTracking ? _hotels.AsNoTracking() : _hotels;
        }

        public async Task<HotelEntity> GetAsync(int id, bool asNoTracking = false)
        {
            return asNoTracking
                ? await _hotels.AsNoTracking().FirstOrDefaultAsync(hotel => hotel.Id == id)
                : await _hotels.FirstOrDefaultAsync(hotel => hotel.Id == id);
        }

        public async Task<HotelEntity> GetAsync(string name, bool asNoTracking = false)
        {
            return asNoTracking
                ? await _hotels.AsNoTracking().FirstOrDefaultAsync(hotel => hotel.Name == name)
                : await _hotels.FirstOrDefaultAsync(hotel => hotel.Name == name);
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate, bool asNoTracking = false)
        {
            return asNoTracking ? _hotels.AsNoTracking().Where(predicate) : _hotels.Where(predicate);
        }

        public async Task<HotelEntity> CreateAsync(HotelEntity hotel)
        {
            var addedHotelEntry = await _hotels.AddAsync(hotel);
            await _db.SaveChangesAsync();

            return addedHotelEntry.Entity;
        }

        public async Task<HotelEntity> UpdateAsync(HotelEntity newItem)
        {
            var addedHotelEntry = _hotels.Update(newItem);
            await _db.SaveChangesAsync();

            return addedHotelEntry.Entity;
        }

        public async Task<HotelEntity> DeleteAsync(int id)
        {
            var hotel = await _hotels.FindAsync(id);

            if (hotel != null)
            {
                _hotels.Remove(hotel);
                await _db.SaveChangesAsync();
            }

            return hotel;
        }
    }
}