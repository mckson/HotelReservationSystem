using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly HotelContext _db;
        private readonly DbSet<LocationEntity> _locations;

        public LocationRepository(HotelContext context)
        {
            _db = context;
            _locations = context.Locations;
        }

        public IEnumerable<LocationEntity> GetAll() => _locations;

        public async Task<LocationEntity> GetAsync(int id) =>
            await _locations.FirstOrDefaultAsync(location => location.Id == id);

        public async Task<LocationEntity> GetAsync(
            string country,
            string region,
            string city,
            string street,
            int building)
        {
            return await _locations.FirstOrDefaultAsync(location =>
                location.Country == country &&
                location.Region == region &&
                location.City == city &&
                location.Street == street &&
                location.BuildingNumber == building);
        }

        public IEnumerable<LocationEntity> Find(Func<LocationEntity, bool> predicate) =>
            _locations.Where(predicate);

        public async Task<LocationEntity> CreateAsync(LocationEntity location)
        {
            var addedLocationEntry = await _locations.AddAsync(location);
            await _db.SaveChangesAsync();

            return addedLocationEntry.Entity;
        }

        public async Task<LocationEntity> UpdateAsync(LocationEntity newLocation)
        {
            var addedLocationEntry = _locations.Update(newLocation);
            await _db.SaveChangesAsync();

            return addedLocationEntry.Entity;
        }

        public async Task<LocationEntity> DeleteAsync(int id)
        {
            var location = await _locations.FindAsync(id);

            if (location != null)
            {
                _locations.Remove(location);
                await _db.SaveChangesAsync();
            }

            return location;
        }
    }
}
