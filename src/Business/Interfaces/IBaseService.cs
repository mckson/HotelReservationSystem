using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IBaseService<TModel>
    {
        public Task<TModel> CreateAsync(TModel userModel);

        public Task<TModel> GetAsync(int id);

        public Task<TModel> DeleteAsync(int id);
    }
}
