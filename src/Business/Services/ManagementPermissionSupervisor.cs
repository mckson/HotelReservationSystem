using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ManagementPermissionSupervisor : IManagementPermissionSupervisor
    {
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ManagementPermissionSupervisor(ILogger logger, IHotelRepository hotelRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _hotelRepository = hotelRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CheckHotelManagementPermissionAsync(Guid hotelId)
        {
            _logger.Debug($"Permissions for managing hotel with hotelId {hotelId} is checking");

            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role))
                .Any(role => role.Value.Equals(Roles.Admin, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            var hotelEntity = await _hotelRepository.GetAsync(hotelId) ??
                              throw new BusinessException("No hotel with such hotelId", ErrorStatus.NotFound);

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
                Guid.TryParse(hotel.Value, out var id);

                if (!id.Equals(hotelEntity.Id))
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

            _logger.Debug($"Permissions for managing hotel with hotelId {hotelId} checked");
        }
    }
}
