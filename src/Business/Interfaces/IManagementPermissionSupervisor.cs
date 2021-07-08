using System;
using System.Threading.Tasks;

namespace HotelReservation.Business.Interfaces
{
    public interface IManagementPermissionSupervisor
    {
        Task CheckHotelManagementPermissionAsync(Guid hotelId);

        void CheckReservationManagementPermission(string reservationEmail);
    }
}
