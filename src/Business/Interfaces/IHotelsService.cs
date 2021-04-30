using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Business.Models.RequestModels;
using HotelReservation.Business.Models.ResponseModels;
using HotelReservation.Data.Entities;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelsService : IBaseService<HotelEntity, HotelRequestModel, HotelResponseModel>
    {
        public Task<IEnumerable<HotelResponseModel>> GetHotelsAsync();
    }
}
