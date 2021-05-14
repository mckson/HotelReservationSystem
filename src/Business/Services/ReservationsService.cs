using System;
using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IRepository<ReservationEntity> _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public ReservationsService(
            IRepository<ReservationEntity> reservationRepository,
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<ReservationModel> CreateAsync(ReservationModel reservationModel, IEnumerable<Claim> userClaims)
        {
            await CheckHotelRoomsServicesExistenceAsync(reservationModel);

            reservationModel.ReservedTime = DateTime.Now;
            reservationModel.UserId = userClaims.FirstOrDefault(claim => claim.Type == "id")?.Value;    // add exception

            var reservationEntity = _mapper.Map<ReservationEntity>(reservationModel);

            var createdReservationEntity = await _reservationRepository.CreateAsync(reservationEntity);
            var createdReservationModel = _mapper.Map<ReservationModel>(createdReservationEntity);

            return createdReservationModel;
        }

        public async Task<ReservationModel> GetAsync(int id, IEnumerable<Claim> userClaims)
        {
            var reservationEntity = await _reservationRepository.GetAsync(id) ??
                                    throw new BusinessException($"No reservation with such id: {id}", ErrorStatus.NotFound);

            await CheckReservationManagementPermission(reservationEntity.UserId, userClaims);

            var reservationModel = _mapper.Map<ReservationModel>(reservationEntity);

            return reservationModel;
        }

        public async Task<ReservationModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            var checkReservationEntity = await _reservationRepository.GetAsync(id, true) ??
                                         throw new BusinessException(
                                             $"No reservation with such id: {id}",
                                             ErrorStatus.NotFound);

            await CheckReservationManagementPermission(checkReservationEntity.UserId, userClaims);

            var deletedReservationEntity = await _reservationRepository.DeleteAsync(id);
            var deletedReservationModel = _mapper.Map<ReservationModel>(deletedReservationEntity);

            return deletedReservationModel;
        }

        public async Task<IEnumerable<ReservationModel>> GetAllReservationsAsync(IEnumerable<Claim> userClaims)
        {
            await CheckReservationManagementPermission(null, userClaims);   // admin only, change

            var reservationEntities = _reservationRepository.GetAll();
            var reservationModels = _mapper.Map<IEnumerable<ReservationModel>>(reservationEntities);

            return reservationModels;
        }

        private async Task CheckHotelRoomsServicesExistenceAsync(ReservationModel reservationModel)
        {
            var checkHotelEntity = await _hotelRepository.GetAsync(reservationModel.HotelId) ??
                                   throw new BusinessException($"No hotel with such id {reservationModel.HotelId}", ErrorStatus.NotFound);

            foreach (var room in reservationModel.ReservationRooms)
            {
                var checkRoomEntity = checkHotelEntity.Rooms.FirstOrDefault(r => r.Id == room.RoomId) ??
                                      throw new BusinessException($"No room with such id: {room.RoomId}", ErrorStatus.NotFound);
            }

            foreach (var service in reservationModel.ReservationServices)
            {
                var checkServiceEntity = checkHotelEntity.Services.FirstOrDefault(s => s.Id == service.ServiceId) ??
                                         throw new BusinessException($"No service with such id: {service.ServiceId}", ErrorStatus.NotFound);
            }
        }

        private async Task CheckReservationManagementPermission(string reservationUserId, IEnumerable<Claim> userClaims)
        {
            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            var userClaimId = claims.FirstOrDefault(claim => claim.Type == "id")?.Value;

            if (reservationUserId != userClaimId)
            {
                throw new BusinessException(
                    "You have no permissions to manage this reservation",
                    ErrorStatus.AccessDenied);
            }
        }
    }
}
