using System;
using HotelReservation.Business.Models;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IHotelImagesService : IBaseService<HotelImageModel>
    {
        Task<HotelImageModel> ChangeImageToMainAsync(Guid id);
    }
}
