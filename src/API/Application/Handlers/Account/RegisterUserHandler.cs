using HotelReservation.API.Application.Commands.Account;
using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Options;
using HotelReservation.Business;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Account
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly UnregisteredUserOptions _unregisteredUserOptions;
        private readonly IMediator _mediator;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IOptions<UnregisteredUserOptions> unregisteredUserOptions,
            IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
            _unregisteredUserOptions = unregisteredUserOptions.Value;
        }

        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUserEntity is { IsRegistered: true })
            {
                throw new BusinessException("User with such email already exists", ErrorStatus.AlreadyExist);
            }

            if (existingUserEntity != null)
            {
                var updateUserCommand = new UpdateUserCommand
                {
                    DateOfBirth = request.DateOfBirth,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    Hotels = null,
                    IsRegistered = true,
                    Id = existingUserEntity.Id,
                    NewPassword = request.Password,
                    LastName = request.LastName,
                    OldPassword = _unregisteredUserOptions.Password,
                    PasswordConfirm = request.PasswordConfirm,
                    PhoneNumber = request.PhoneNumber,
                    Roles = new List<string>
                    {
                        Roles.User
                    },
                    UserName = request.UserName ?? request.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0]
                };

                await _mediator.Send(updateUserCommand, cancellationToken);
            }
            else
            {
                var createUserCommand = new CreateUserCommand
                {
                    DateOfBirth = request.DateOfBirth ?? DateTime.MinValue,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    Hotels = null,
                    IsRegistered = true,
                    Password = request.Password,
                    LastName = request.LastName,
                    PasswordConfirm = request.PasswordConfirm,
                    PhoneNumber = request.PhoneNumber,
                    Roles = new List<string>
                    {
                        Roles.User
                    },
                    UserName = request.UserName ?? request.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0]
                };

                await _mediator.Send(createUserCommand, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
