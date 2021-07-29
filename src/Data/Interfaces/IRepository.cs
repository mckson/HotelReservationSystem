using HotelReservation.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();

        Task<TEntity> GetAsync(Guid id);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            PaginationFilter paginationFilter,
            string orderByPropertyName = null,
            bool? isDescending = null);

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity newEntity);

        Task<TEntity> DeleteAsync(Guid id);
    }
}
