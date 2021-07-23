using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelReservation.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserRepository(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<int> GetCountAsync(Expression<Func<UserEntity, bool>> predicate)
        {
            var count = await _userManager.Users.Where(predicate).CountAsync();
            return count;
        }

        public async Task<IEnumerable<UserEntity>> GetAll()
        {
            var users = _userManager.Users;

            foreach (var user in users)
            {
                await GetRolesForUserAsync(user);
            }

            return users;
        }

        public async Task<UserEntity> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await GetRolesForUserAsync(user);
            }

            return user;
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await GetRolesForUserAsync(user);
            }

            return user;
        }

        public async Task<UserEntity> GetByNameAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                await GetRolesForUserAsync(user);
            }

            return user;
        }

        public async Task<IQueryable<UserEntity>> Find(Expression<Func<UserEntity, bool>> predicate)
        {
            var users = _userManager.Users.Where(predicate);

            foreach (var user in users)
            {
                await GetRolesForUserAsync(user);
            }

            return users;
        }

        public async Task<IQueryable<UserEntity>> Find(Expression<Func<UserEntity, bool>> predicate, PaginationFilter paginationFilter)
        {
            var users = _userManager.Users.Where(predicate)
                .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);

            foreach (var user in users)
            {
                await GetRolesForUserAsync(user);
            }

            return users;
        }

        public async Task<bool> CreateAsync(UserEntity user)
        {
            var result = await _userManager.CreateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(UserEntity user)
        {
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> AddToRoleAsync(UserEntity user, string role)
        {
            if (string.Equals(role, Roles.Admin, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(role, Roles.Manager, StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(role, Roles.User, StringComparison.InvariantCultureIgnoreCase))
            {
                var result = await _userManager.AddToRoleAsync(user, role);

                return result.Succeeded;
            }

            return false;
        }

        private async Task GetRolesForUserAsync(UserEntity user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            user.Roles = roles;
        }
    }
}
