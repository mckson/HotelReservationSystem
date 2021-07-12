using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<int> GetCountAsync(Expression<Func<UserEntity, bool>> predicate);

        Task<IEnumerable<UserEntity>> GetAll();

        Task<UserEntity> GetByIdAsync(Guid id);

        Task<UserEntity> GetByEmailAsync(string email);

        Task<IQueryable<UserEntity>> Find(Expression<Func<UserEntity, bool>> predicate);

        Task<IQueryable<UserEntity>> Find(
            Expression<Func<UserEntity, bool>> predicate,
            PaginationFilter paginationFilter);

        Task<bool> CreateAsync(UserEntity entity);

        Task<bool> UpdateAsync(UserEntity newEntity);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> AddToRoleAsync(UserEntity user, string role);
    }
}
