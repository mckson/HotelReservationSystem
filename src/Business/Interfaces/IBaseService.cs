using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService<TEntity, TModel>
    {
        public Task<TModel> CreateAsync(TModel userModel, IEnumerable<Claim> userClaims);

        public Task<TModel> GetAsync(int id);

        public Task<TModel> DeleteAsync(int id, IEnumerable<Claim> userClaims);

        public Task<TModel> UpdateAsync(int id, TModel updatingRoomModel, IEnumerable<Claim> userClaims);
    }
}
