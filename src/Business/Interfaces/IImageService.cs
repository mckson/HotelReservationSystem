using HotelReservation.Business.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IImageService
    {
        Task<ImageModel> CreateAsync(ImageModel imageModel, IEnumerable<Claim> userClaims);

        Task<ImageModel> GetAsync(int id);

        Task<ImageModel> DeleteAsync(int id, IEnumerable<Claim> userClaims);
    }
}
