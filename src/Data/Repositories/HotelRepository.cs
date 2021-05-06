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
        private readonly DbSet<HotelEntity> _hotels;

        public HotelRepository(HotelContext context)
        {
            _db = context;
            _hotels = context.Hotels;
        }

        public IEnumerable<HotelEntity> GetAll() => _hotels;

        public async Task<IEnumerable<HotelEntity>> GetAllAsync() => await Task.Run(GetAll);

        public HotelEntity Get(int id) => _hotels.FirstOrDefault(hotel => hotel.Id == id);

        public async Task<HotelEntity> GetAsync(int id) => await Task.Run(() => Get(id));

        public async Task<HotelEntity> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate) => _hotels.Where(predicate);

        public async Task<IEnumerable<HotelEntity>> FindAsync(Func<HotelEntity, bool> predicate) =>
            await Task.Run(() => Find(predicate));

        public void Create(HotelEntity hotel)
        {
            _hotels.Add(hotel);
            _db.SaveChanges();
        }

        public async Task CreateAsync(HotelEntity hotel)
        {
            await _hotels.AddAsync(hotel);
            await _db.SaveChangesAsync();
        }

        // change implementation
        public void Update(HotelEntity newHotel)
        {
            _hotels.Update(newHotel);
            _db.SaveChanges();
        }

        public async Task UpdateAsync(HotelEntity newItem)
        {
            _hotels.Update(newItem);
            await _db.SaveChangesAsync();
        }

        public HotelEntity Delete(int id)
        {
            var hotel = _hotels.Find(id);

            if (hotel != null)
            {
                _hotels.Remove(hotel);
                _db.SaveChanges();
            }

            return hotel;
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