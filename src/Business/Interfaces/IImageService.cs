using HotelReservation.Business.Models;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IImageService : IBaseService<ImageModel>
    {
        Task<ImageModel> ChangeImageToMainAsync(int id);
    }
}
