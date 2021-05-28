using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        IEnumerable<TEntity> GetAll();

        Task<TEntity> GetAsync(int id);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity newEntity);

        Task<TEntity> DeleteAsync(int id);
    }
}
