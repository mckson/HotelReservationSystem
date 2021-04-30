using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService<TEntity, TRequestModel, TResponseModel>
    {
        public Task<TResponseModel> CreateAsync(TRequestModel requestModel);
        public Task<TResponseModel> GetAsync(int id);
        public Task DeleteAsync(int id);
        public Task<TResponseModel> UpdateAsync(TRequestModel updateModel);
    }
}
