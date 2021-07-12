using AutoMapper;
using HotelReservation.API.Application.Commands.Account;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Account
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, TokenResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IMapper _mapper;

        public RefreshTokenHandler(
            IUserRepository userRepository,
            IMapper mapper,
            ITokenService tokenService,
            IAuthenticationHelper authenticationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<TokenResponseModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userEntity = (await _userRepository.Find(u => u.RefreshToken.Token == request.Token)).FirstOrDefault();

            if (userEntity == null)
            {
                throw new BusinessException(
                    "Invalid refresh token. User for such refresh token does not exist",
                    ErrorStatus.NotFound);
            }

            var refreshToken = userEntity.RefreshToken;

            if (refreshToken == null)
            {
                throw new BusinessException("Refresh token for current user is not found", ErrorStatus.NotFound);
            }

            if (!refreshToken.IsActive)
            {
                throw new BusinessException(
                    "Invalid refresh token. Current refresh token is unavailable now",
                    ErrorStatus.NotFound);
            }

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            userEntity.RefreshToken = _mapper.Map<RefreshTokenEntity>(newRefreshToken);

            await _userRepository.UpdateAsync(userEntity);

            var tokenResponse = _mapper.Map<TokenResponseModel>(userEntity);

            var claims = await _authenticationHelper.GetIdentityAsync(userEntity.Email);
            var jwtToken = _tokenService.GenerateJwtToken(claims);

            tokenResponse.JwtToken = jwtToken;

            return tokenResponse;
        }
    }
}
