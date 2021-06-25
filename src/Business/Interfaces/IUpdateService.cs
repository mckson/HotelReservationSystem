using System;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IUpdateService<TModel>
    {
        public Task<TModel> UpdateAsync(Guid id, TModel updatingRoomModel);
    }
}
