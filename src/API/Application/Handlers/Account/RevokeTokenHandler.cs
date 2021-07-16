using HotelReservation.API.Application.Commands.Account;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Account
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IUserRepository _userRepository;

        public RevokeTokenHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                throw new BusinessException(
                    "User with such refresh token is already logged out",
                    ErrorStatus.IncorrectInput);
            }

            var user = (await _userRepository.Find(u => u.RefreshToken.Token == request.Token)).FirstOrDefault();

            if (user == null)
            {
                throw new BusinessException(
                    "Invalid refresh token. User for such refresh token does not exist",
                    ErrorStatus.NotFound);
            }

            var refreshToken = user.RefreshToken;

            if (!refreshToken.IsActive)
            {
                throw new BusinessException(
                    "Invalid refresh token. Current refresh token is already expired",
                    ErrorStatus.NotFound);
            }

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
