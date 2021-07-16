using AutoMapper;
using HotelReservation.API.Application.Commands.Reservation;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ReservationBriefResponseModel>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationHelper _reservationHelper;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            IReservationHelper reservationHelper,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationHelper = reservationHelper;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ReservationBriefResponseModel> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservationEntity = _mapper.Map<ReservationEntity>(request);

            var userEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (userEntity != null)
            {
                reservationEntity.UserId = userEntity.Id;
            }

            await _reservationHelper.CheckHotelRoomsServicesExistenceAsync(reservationEntity);

            reservationEntity.ReservedTime = DateTime.Now; // into entity

            var createdReservationEntity = await _reservationRepository.CreateAsync(reservationEntity);
            var createdReservationResponse = _mapper.Map<ReservationBriefResponseModel>(createdReservationEntity);

            return createdReservationResponse;
        }
    }
}
