using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public class LocationRepository : IRepository<LocationEntity>
    {
        private readonly HotelContext _db;

        public LocationRepository(HotelContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<LocationEntity>> FindAsync(Func<LocationEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public void Create(LocationEntity location)
        {
            _db.Locations.Add(location);
        }

        public async Task CreateAsync(LocationEntity location)
        {
            await _db.Locations.AddAsync(location);
        }

        public async Task UpdateAsync(LocationEntity newLocation)
        {
            var oldLocation = await _db.Locations.FindAsync(newLocation);

            if (oldLocation != null)
                oldLocation = newLocation;
        }

        public void Delete(int id)
        {
            var location = _db.Locations.Find(id);

            if (location != null)
                _db.Locations.Remove(location);
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _db.Locations.FindAsync(id);

            if (location != null)
                _db.Locations.Remove(location);
        }

        public async Task<LocationEntity> GetAsync(int id)
        {
            return await _db.Locations.FindAsync(id);
        }

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate)
        {
            return _db.Locations.Where(predicate);
        }

        public async Task<IEnumerable<LocationEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public LocationEntity Get(int id)
        {
            return _db.Locations.Find(id);
        }

        public IEnumerable<LocationEntity> GetAll()
        {
            return _db.Locations;
        }

        public void Update(LocationEntity newLocation)
        {
            var oldLocation = _db.Locations.Find(newLocation);

            if (oldLocation != null)
                oldLocation = newLocation;
        }
    }
}
