using AutoMapper;
using HotelReservation.API.Commands.Reservation;
using HotelReservation.API.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Reservation
{
    public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ReservationBriefResponseModel>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationHelper _reservationHelper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            IReservationHelper reservationHelper,
            ILogger logger,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationHelper = reservationHelper;
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ReservationBriefResponseModel> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug("Reservation is creating");

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

            _logger.Debug($"Reservation {createdReservationResponse.Id} created");

            return createdReservationResponse;
        }
    }
}
