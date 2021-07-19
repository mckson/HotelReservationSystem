using AutoMapper;
using HotelReservation.API.Application.Commands.Reservation;
using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Options;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
        private readonly IMediator _mediator;
        private readonly UnregisteredUserOptions _unregisteredUserOptions;

        public CreateReservationHandler(
            IReservationRepository reservationRepository,
            IReservationHelper reservationHelper,
            IMapper mapper,
            IUserRepository userRepository,
            IMediator mediator,
            IOptions<UnregisteredUserOptions> unregisteredUserOptions)
        {
            _reservationRepository = reservationRepository;
            _reservationHelper = reservationHelper;
            _mapper = mapper;
            _userRepository = userRepository;
            _mediator = mediator;
            _unregisteredUserOptions = unregisteredUserOptions.Value;
        }

        public async Task<ReservationBriefResponseModel> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var reservationEntity = _mapper.Map<ReservationEntity>(request);

            var userEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (userEntity != null)
            {
                reservationEntity.Email = userEntity.Email;
            }
            else
            {
                await CreateUnregisteredUserAsync(request);
            }

            await _reservationHelper.CheckHotelRoomsServicesExistenceAsync(reservationEntity);

            reservationEntity.ReservedTime = DateTime.Now;

            var createdReservationEntity = await _reservationRepository.CreateAsync(reservationEntity);
            var createdReservationResponse = _mapper.Map<ReservationBriefResponseModel>(createdReservationEntity);

            return createdReservationResponse;
        }

        private async Task CreateUnregisteredUserAsync(CreateReservationCommand createReservation)
        {
            var command = new CreateUserCommand
            {
                Email = createReservation.Email,
                DateOfBirth = DateTime.MinValue,
                FirstName = createReservation.FirstName,
                LastName = createReservation.LastName,
                Hotels = null,
                IsRegistered = false,
                Password = _unregisteredUserOptions.Password,
                PasswordConfirm = _unregisteredUserOptions.Password,
                PhoneNumber = _unregisteredUserOptions.PhoneNumber,
                Roles = new List<string>
                {
                    Roles.User
                }
            };

            await _mediator.Send(command);
        }
    }
}
