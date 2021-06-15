using HotelReservation.Business.Constants;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business
{
    public class ManagementPermissionSupervisor
    {
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;

        public ManagementPermissionSupervisor(ILogger logger, IHotelRepository hotelRepository)
        {
            _logger = logger;
            _hotelRepository = hotelRepository;
        }

        public async Task CheckHotelManagementPermissionAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Permissions for managing hotel with id {id} is checking");

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role))
                .Any(role => role.Value.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var hotelEntity = await _hotelRepository.GetAsync(id) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            var hotels = claims.FindAll(claim => claim.Type == ClaimNames.Hotels);

            if (hotels.Count == 0)
            {
                throw new BusinessException(
                    "You have no permissions to manage hotels. Ask application admin to take that permission",
                    ErrorStatus.AccessDenied);
            }

            var accessDenied = true;
            foreach (var hotel in hotels)
            {
                int.TryParse(hotel.Value, out var hotelId);

                if (hotelId != hotelEntity.Id)
                    continue;

                accessDenied = false;
                break;
            }

            if (accessDenied)
            {
                throw new BusinessException(
                    $"You have no permission to manage hotel {hotelEntity.Name}. Ask application admin about permissions",
                    ErrorStatus.AccessDenied);
            }

            _logger.Debug($"Permissions for managing hotel with id {id} checked");

            // return !accessDenied;
        }
    }
}
