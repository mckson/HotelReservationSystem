using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class LocationRepository : IRepository<LocationEntity>
    {
        private readonly HotelContext _db;
        private readonly DbSet<LocationEntity> _locations;

        public LocationRepository(HotelContext context)
        {
            _db = context;
            _locations = context.Locations;
        }

        public IEnumerable<LocationEntity> GetAll() => _locations;

        public async Task<IEnumerable<LocationEntity>> GetAllAsync() => await Task.Run(GetAll);

        public LocationEntity Get(int id) => _locations.FirstOrDefault(location => location.Id == id);

        public async Task<LocationEntity> GetAsync(int id) => await Task.Run(() => Get(id));

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate) => _locations.Where(predicate);

        public async Task<IEnumerable<LocationEntity>> FindAsync(Func<LocationEntity, bool> predicate) => await Task.Run(() => Find(predicate));

        public void Create(LocationEntity location)
        {
            _locations.Add(location);
            _db.SaveChanges();
        }

        public async Task CreateAsync(LocationEntity location)
        {
            await _locations.AddAsync(location);
            await _db.SaveChangesAsync();
        }

        public void Update(LocationEntity newLocation)
        {
            var oldLocation = _locations.Find(newLocation);

            if (oldLocation != null)
            {
                oldLocation = newLocation;
                _db.SaveChanges();
            }
        }

        // change implementation
        public async Task UpdateAsync(LocationEntity newLocation)
        {
            var oldLocation = await _locations.FindAsync(newLocation);

            if (oldLocation != null)
            {
                oldLocation = newLocation;
                await _db.SaveChangesAsync();
            }
        }

        public void Delete(int id)
        {
            var location = _locations.Find(id);

            if (location != null)
            {
                _locations.Remove(location);
                _db.SaveChanges();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _locations.FindAsync(id);

            if (location != null)
            {
                _locations.Remove(location);
                await _db.SaveChangesAsync();
            }
        }
    }
}
