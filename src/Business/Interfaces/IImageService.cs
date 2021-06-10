using HotelReservation.Business.Models;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IImageService
    {
        Task<ImageModel> CreateAsync(ImageModel imageModel);

        Task<ImageModel> GetAsync(int id);

        Task<ImageModel> DeleteAsync(int id);
    }
}
