using Castle.Core.Internal;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private readonly HotelContext _db;
        private readonly ISortHelper<TEntity> _sortHelper;

        protected BaseRepository(HotelContext context, ISortHelper<TEntity> sortHelper)
        {
            _db = context;
            DbSet = context.Set<TEntity>();
            _sortHelper = sortHelper;
        }

        protected DbSet<TEntity> DbSet { get; }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).CountAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        public async Task<TEntity> GetAsync(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<TEntity> Find(
            Expression<Func<TEntity, bool>> predicate,
            PaginationFilter paginationFilter,
            string orderByPropertyName = null,
            bool? isDescending = null)
        {
            var filteredEntities = DbSet.Where(predicate);
            var orderedEntities = orderByPropertyName != null
                ? _sortHelper.ApplySort(filteredEntities, orderByPropertyName, isDescending ?? false)
                : filteredEntities;
            var pagedEntities = orderedEntities.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

            return pagedEntities;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var addedEntityEntry = await DbSet.AddAsync(entity);
            await _db.SaveChangesAsync();

            return addedEntityEntry.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity newEntity)
        {
            var updatedEntityEntry = DbSet.Update(newEntity);
            await _db.SaveChangesAsync();

            return updatedEntityEntry.Entity;
        }

        public async Task<TEntity> DeleteAsync(Guid id)
        {
            var deletingEntity = await DbSet.FindAsync(id);

            if (deletingEntity != null)
            {
                DbSet.Remove(deletingEntity);
                await _db.SaveChangesAsync();
            }

            return deletingEntity;
        }
    }
}
