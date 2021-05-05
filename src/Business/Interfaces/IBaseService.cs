using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService/*<TModel, TRequestModel, TResponseModel>*/<TEntity, TModel>
    {
        /*public Task<Tmodel> CreateAsync(TRequestModel requestModel);

        public Task<TResponseModel> GetAsync(int id);

        public Task DeleteAsync(int id);

        public Task<TResponseModel> UpdateAsync(int id, TRequestModel requestModel);*/
        public Task<TModel> CreateAsync(TModel model);

        public Task<TModel> GetAsync(int id);

        public Task DeleteAsync(int id);

        public Task<TModel> UpdateAsync(int id, TModel model);
    }
}
