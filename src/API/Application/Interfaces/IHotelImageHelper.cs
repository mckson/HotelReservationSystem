using HotelReservation.Data.Entities;
using System;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Interfaces
{
    public interface IHotelImageHelper
    {
        Task ChangeHotelMainImageAsync(Guid hotelId, ImageEntity newImage);
    }
}
