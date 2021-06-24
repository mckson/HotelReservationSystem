using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IUpdateService<TModel>
    {
        public Task<TModel> UpdateAsync(int id, TModel updatingRoomModel);
    }
}
