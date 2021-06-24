using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationsService(
            IReservationRepository reservationRepository,
            IHotelRepository hotelRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ILogger logger)
        {
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReservationModel> CreateAsync(ReservationModel reservationModel)
        {
            _logger.Debug("Reservation is creating");

            await CheckHotelRoomsServicesExistenceAsync(reservationModel);

            reservationModel.ReservedTime = DateTime.Now; // into entity

            var reservationEntity = _mapper.Map<ReservationEntity>(reservationModel);

            var createdReservationEntity = await _reservationRepository.CreateAsync(reservationEntity);
            var createdReservationModel = _mapper.Map<ReservationModel>(createdReservationEntity);

            _logger.Debug($"Reservation {createdReservationModel.Id} created");

            return createdReservationModel;
        }

        public async Task<ReservationModel> GetAsync(int id)
        {
            _logger.Debug($"Reservation {id} is requesting");

            var reservationEntity = await _reservationRepository.GetAsync(id) ??
                                    throw new BusinessException($"No reservation with such id: {id}", ErrorStatus.NotFound);

            CheckReservationManagementPermission(reservationEntity.Email);

            var reservationModel = _mapper.Map<ReservationModel>(reservationEntity);

            _logger.Debug($"Reservation {id} requested");

            return reservationModel;
        }

        public async Task<ReservationModel> DeleteAsync(int id)
        {
            _logger.Debug($"Reservation {id} is deleting");

            var checkReservationEntity = await _reservationRepository.GetAsync(id) ??
                                         throw new BusinessException(
                                             $"No reservation with such id: {id}",
                                             ErrorStatus.NotFound);

            CheckReservationManagementPermission(checkReservationEntity.Email);

            var deletedReservationEntity = await _reservationRepository.DeleteAsync(id);
            var deletedReservationModel = _mapper.Map<ReservationModel>(deletedReservationEntity);

            _logger.Debug($"Reservation {id} deleted");

            return deletedReservationModel;
        }

        public IEnumerable<ReservationModel> GetAllReservations(IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Reservations are requesting");

            CheckReservationManagementPermission(null);   // admin only, change

            var reservationEntities = _reservationRepository.GetAll();
            var reservationModels = _mapper.Map<IEnumerable<ReservationModel>>(reservationEntities);

            _logger.Debug("Reservations requested");

            return reservationModels;
        }

        public IEnumerable<ReservationModel> GetReservationsByEmail(string email)
        {
            _logger.Debug($"Reservations of {email} are requesting");

            var reservationEntities = _reservationRepository.Find(reservation => reservation.Email.Equals(email));
            var reservationModels = _mapper.Map<IEnumerable<ReservationModel>>(reservationEntities);

            _logger.Debug($"Reservations of {email} requested");

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
                        (reservation.DateIn >= reservationModel.DateIn && reservation.DateIn < reservationModel.DateOut) ||
                        (reservation.DateOut > reservationModel.DateIn && reservation.DateOut <= reservationModel.DateOut));

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

        private void CheckReservationManagementPermission(string reservationEmail)
        {
            _logger.Debug("Permissions is checking");

            var userClaims = _httpContextAccessor.HttpContext.User.Claims;

            var claims = userClaims.ToList();
            if (claims.Where(claim => claim.Type.Equals(ClaimTypes.Role)).Any(role => role.Value.ToUpper() == "ADMIN"))
                return;

            var userClaimEmail = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Email))?.Value;

            if (!reservationEmail.Equals(userClaimEmail))
            {
                throw new BusinessException(
                    "You have no permissions to manage this reservation",
                    ErrorStatus.AccessDenied);
            }

            _logger.Debug("Permissions checked");
        }
    }
}
