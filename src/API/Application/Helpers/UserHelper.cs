using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HotelReservation.API.Application.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserHelper(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<ReservationEntity> GetUserReservations(string email)
        {
            var reservationEntities = _reservationRepository.Find(reservation => reservation.Email.Equals(email)).ToList();

            return reservationEntities;
        }

        public bool IsCurrentUser(Guid id)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            var currentUserId = claims.FirstOrDefault(claim => claim.Type.Equals(ClaimNames.Id))?.Value;
            var currentUserIdGuid = Guid.Parse(currentUserId ?? string.Empty);
            var isCurrentIdEqualsToId = id.Equals(currentUserIdGuid);

            return isCurrentIdEqualsToId;
        }
    }
}
