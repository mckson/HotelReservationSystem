using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        TEntity Get(int id);

        Task<TEntity> GetAsync(int id);

        Task<TEntity> GetAsync(string name);

        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);

        Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate);

        void Create(TEntity item);

        Task CreateAsync(TEntity item);

        void Update(TEntity newItem);

        Task UpdateAsync(TEntity newItem);

        TEntity Delete(int id);

        Task<TEntity> DeleteAsync(int id);
    }
}
