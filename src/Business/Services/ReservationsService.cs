using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
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
        private readonly ILogger _logger;

        public ReservationsService(
            IRepository<ReservationEntity> reservationRepository,
            IHotelRepository hotelRepository,
            IMapper mapper,
            ILogger logger)
        {
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReservationModel> CreateAsync(ReservationModel reservationModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Reservation is creating");

            await CheckHotelRoomsServicesExistenceAsync(reservationModel);

            reservationModel.ReservedTime = DateTime.Now;
            var parsedId = Guid.Parse(userClaims.FirstOrDefault(claim => claim.Type == "id")?.Value);

            if (parsedId.Equals(Guid.Empty))
            {
                throw new BusinessException(
                    "Cannot maker reservation when user is not authorized",
                    ErrorStatus.AccessDenied);
            }

            reservationModel.User.Id = parsedId;

            var reservationEntity = _mapper.Map<ReservationEntity>(reservationModel);

            var createdReservationEntity = await _reservationRepository.CreateAsync(reservationEntity);
            var createdReservationModel = _mapper.Map<ReservationModel>(createdReservationEntity);

            _logger.Debug($"Reservation {createdReservationModel.Id} created");

            return createdReservationModel;
        }

        public async Task<ReservationModel> GetAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Reservation {id} is requesting");

            var reservationEntity = await _reservationRepository.GetAsync(id) ??
                                    throw new BusinessException($"No reservation with such id: {id}", ErrorStatus.NotFound);

            CheckReservationManagementPermission(reservationEntity.User.Id, userClaims);

            var reservationModel = _mapper.Map<ReservationModel>(reservationEntity);

            _logger.Debug($"Reservation {id} requested");

            return reservationModel;
        }

        public async Task<ReservationModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Reservation {id} is deleting");

            var checkReservationEntity = await _reservationRepository.GetAsync(id, true) ??
                                         throw new BusinessException(
                                             $"No reservation with such id: {id}",
                                             ErrorStatus.NotFound);

            CheckReservationManagementPermission(checkReservationEntity.User.Id, userClaims);

            var deletedReservationEntity = await _reservationRepository.DeleteAsync(id);
            var deletedReservationModel = _mapper.Map<ReservationModel>(deletedReservationEntity);

            _logger.Debug($"Reservation {id} deleted");

            return deletedReservationModel;
        }

        public IEnumerable<ReservationModel> GetAllReservations(IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Reservations is requesting");

            CheckReservationManagementPermission(Guid.Empty, userClaims);   // admin only, change

            var reservationEntities = _reservationRepository.GetAll();
            var reservationModels = _mapper.Map<IEnumerable<ReservationModel>>(reservationEntities);

            _logger.Debug("Reservations requested");

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

                var checkReservation = checkRoomEntity.ReservationRooms.Select(rr => rr.Reservation).FirstOrDefault(
                    reservation =>
                        (reservation.DateIn >= reservationModel.DateIn && reservation.DateIn <= reservationModel.DateOut) ||
                        (reservation.DateOut >= reservationModel.DateIn && reservation.DateOut <= reservationModel.DateOut));

                if (checkReservation != null)
                {
                    throw new BusinessException(
                        $"Room with id {checkRoomEntity.Id} has been already reserved from {checkReservation.DateIn} to {checkReservation.DateOut}",
                        ErrorStatus.IncorrectInput);
                }
            }

            foreach (var service in reservationModel.ReservationServices)
            {
                var unused = checkHotelEntity.Services.FirstOrDefault(s => s.Id == service.ServiceId) ??
                             throw new BusinessException(
                                 $"No service with such id: {service.ServiceId}",
                                 ErrorStatus.NotFound);
            }
        }

        private void CheckReservationManagementPermission(Guid reservationUserId, IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Permissions is checking");

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            var userClaimId = claims.FirstOrDefault(claim => claim.Type == "id")?.Value;

            if (reservationUserId.Equals(userClaimId))
            {
                throw new BusinessException(
                    "You have no permissions to manage this reservation",
                    ErrorStatus.AccessDenied);
            }

            _logger.Debug("Permissions checked");
        }
    }
}
