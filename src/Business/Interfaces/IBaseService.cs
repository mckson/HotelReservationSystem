using System;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService<TModel>
    {
        public Task<TModel> CreateAsync(TModel userModel);

        public Task<TModel> GetAsync(Guid id);

        public Task<TModel> DeleteAsync(Guid id);
    }
}
