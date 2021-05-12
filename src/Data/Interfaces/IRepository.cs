using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll(bool asNoTracking = false);

        Task<TEntity> GetAsync(int id, bool asNoTracking = false);

        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate, bool asNoTracking = false);

        Task<TEntity> CreateAsync(TEntity item);

        Task<TEntity> UpdateAsync(TEntity newItem);

        Task<TEntity> DeleteAsync(int id);
    }
}
