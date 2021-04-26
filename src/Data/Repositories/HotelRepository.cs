using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

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
                .Select(hotel => new HotelEntity
                {
                    Id = hotel.Id,
                    Name = hotel.Name,
                    Location = hotel.Location,
                    Rooms = hotel.Rooms,
                    RoomsNumber = hotel.RoomsNumber,
                    EmptyRoomsNumber = hotel.EmptyRoomsNumber
                });
        }

        public async Task<IEnumerable<HotelEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public HotelEntity Get(int id)
        {
            var hotel = _db.Hotels.Find(id);
            return new HotelEntity
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Location = hotel.Location,
                Rooms = hotel.Rooms,
                RoomsNumber = hotel.RoomsNumber,
                EmptyRoomsNumber = hotel.EmptyRoomsNumber
            };
        }

        public async Task<HotelEntity> GetAsync(int id)
        {
            var hotel = await _db.Hotels.FindAsync(id); 
            return new HotelEntity
            {
                Id = hotel.Id,
                Name = hotel.Name,
                Location = hotel.Location,
                Rooms = hotel.Rooms,
                RoomsNumber = hotel.RoomsNumber,
                EmptyRoomsNumber = hotel.EmptyRoomsNumber
            };
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

        public void Update(HotelEntity newHotel)
        {
            var oldHotel = _db.Hotels.Find(newHotel.Id);
            oldHotel = newHotel;
        }

        public async Task UpdateAsync(HotelEntity newItem)
        {
            await Task.Run(() => Update(newItem));
        }

        public IEnumerable<HotelEntity> Find(Func<HotelEntity, bool> predicate)
        {
            return _db.Hotels.Where(predicate);
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