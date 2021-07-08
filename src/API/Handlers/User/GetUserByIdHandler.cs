using AutoMapper;
using HotelReservation.API.Interfaces;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.User;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.User
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, ILogger logger, IMapper mapper, IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        public async Task<UserResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"User {request.Id} is requesting");

            var userEntity = await _userRepository.GetByIdAsync(request.Id) ??
                             throw new BusinessException($"User with id {request.Id} does not exist", ErrorStatus.NotFound);

            userEntity.Reservations = _userHelper.GetUserReservations(userEntity.Email);

            var userResponse = _mapper.Map<UserResponseModel>(userEntity);

            _logger.Debug($"User {request.Id} requested");

            return userResponse;
        }
    }
}
