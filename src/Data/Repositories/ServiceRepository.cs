using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class ServiceRepository : IRepository<ServiceEntity>
    {
        private readonly DbSet<ServiceEntity> _services;
        private readonly HotelContext _db;

        public ServiceRepository(HotelContext context)
        {
            _db = context;
            _services = context.Services;
        }

        public IEnumerable<ServiceEntity> GetAll(bool asNoTracking = false)
        {
            return asNoTracking ? _services.AsNoTracking() : _services;
        }

        public async Task<ServiceEntity> GetAsync(int id, bool asNoTracking = false)
        {
            return asNoTracking
                ? await _services.AsNoTracking().FirstOrDefaultAsync(service => service.Id == id)
                : await _services.FirstOrDefaultAsync(service => service.Id == id);
        }

        public IEnumerable<ServiceEntity> Find(Func<ServiceEntity, bool> predicate, bool asNoTracking = false)
        {
            return asNoTracking ? _services.AsNoTracking().Where(predicate) : _services.Where(predicate);
        }

        public async Task<ServiceEntity> CreateAsync(ServiceEntity service)
        {
            var addedServiceEntity = await _services.AddAsync(service);
            await _db.SaveChangesAsync();

            return addedServiceEntity.Entity;
        }

        public async Task<ServiceEntity> UpdateAsync(ServiceEntity newItem)
        {
            var updatedServiceEntity = _services.Update(newItem);
            await _db.SaveChangesAsync();

            return updatedServiceEntity.Entity;
        }

        public async Task<ServiceEntity> DeleteAsync(int id)
        {
            var service = await _services.FindAsync(id);

            if (service != null)
            {
                _services.Remove(service);
                await _db.SaveChangesAsync();
            }

            return service;
        }
    }
}
