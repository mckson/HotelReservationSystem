using HotelReservation.API.Models.RequestModels;
using HotelReservation.Data.Entities;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Interfaces
{
    public interface IHotelHelper
    {
        Task UpdateLocationEntityFieldsAsync(LocationEntity locationToUpdate, LocationRequestModel locationModel);

        bool IsLocationEqual(LocationEntity locationOne, LocationRequestModel locationTwo);
    }
}
