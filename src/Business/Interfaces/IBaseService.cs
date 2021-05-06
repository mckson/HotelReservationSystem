using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService<TEntity, TModel>
    {
        public Task<TModel> CreateAsync(TModel model);

        public Task<TModel> GetAsync(int id);

        public Task<TModel> DeleteAsync(int id);

        public Task<TModel> UpdateAsync(int id, TModel model);
    }
}
