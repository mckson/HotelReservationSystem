using HotelReservation.API.Commands.Account;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Account
{
    public class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public RevokeTokenHandler(IUserRepository userRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                throw new BusinessException(
                    "User with such refresh token is already logged out",
                    ErrorStatus.IncorrectInput);
            }

            _logger.Debug($"Refresh token {request.Token} is revoking");

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

            _logger.Debug($"Refresh token {request.Token} revoked");

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
