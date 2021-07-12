using AutoMapper;
using HotelReservation.API.Application.Commands.Account;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Account
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, TokenResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IMapper _mapper;

        public AuthenticateUserHandler(
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IAuthenticationHelper authenticationHelper,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticationHelper = authenticationHelper;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<TokenResponseModel> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                throw new BusinessException("Password and email cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userRepository.GetByEmailAsync(request.Email) ??
                             throw new BusinessException("User with such email does not exist", ErrorStatus.NotFound);

            if (_passwordHasher.VerifyHashedPassword(
                    userEntity,
                    userEntity.PasswordHash,
                    request.Password) ==
                PasswordVerificationResult.Failed)
                throw new BusinessException("Incorrect password", ErrorStatus.IncorrectInput);

            var claims = await _authenticationHelper.GetIdentityAsync(request.Email);

            var encodedJwt = _tokenService.GenerateJwtToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            userEntity.RefreshToken = refreshToken;
            await _userRepository.UpdateAsync(userEntity);

            var tokenResponse = _mapper.Map<TokenResponseModel>(userEntity);
            tokenResponse.JwtToken = encodedJwt;

            return tokenResponse;
        }
    }
}
