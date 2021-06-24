using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IManagementPermissionSupervisor
    {
        Task CheckHotelManagementPermissionAsync(int hotelId);
    }
}
