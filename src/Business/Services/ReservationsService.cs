using AutoMapper;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IRepository<ReservationEntity> _reservationRepository;
        private readonly IMapper _mapper;

        public ReservationsService(
            IRepository<ReservationEntity> reservationRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<ReservationModel> CreateAsync(ReservationModel userModel, IEnumerable<Claim> userClaims)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ReservationModel> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ReservationModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ReservationModel> UpdateAsync(int id, ReservationModel updatingRoomModel, IEnumerable<Claim> userClaims)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ReservationModel> GetAllReservations()
        {
            throw new System.NotImplementedException();
        }
    }
}
