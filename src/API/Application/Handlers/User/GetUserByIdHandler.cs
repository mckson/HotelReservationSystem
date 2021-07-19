using AutoMapper;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Application.Queries.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponseModel>
    {
        private readonly IUserRepository _userRepository;
        // private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;
        private readonly IAuthenticationHelper _authenticationHelper;

        public GetUserByIdHandler(
            IUserRepository userRepository,
            IMapper mapper,
            // IUserHelper userHelper,
            IAuthenticationHelper authenticationHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            // _userHelper = userHelper;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<UserResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var accessAllowed = _authenticationHelper.CheckGetUserPermission(request.Id);

            if (!accessAllowed)
            {
                throw new BusinessException(
                    "You cannot access data about that user. You can access data only about yourself",
                    ErrorStatus.AccessDenied);
            }

            var userEntity = await _userRepository.GetByIdAsync(request.Id) ??
                             throw new BusinessException($"User with id {request.Id} does not exist", ErrorStatus.NotFound);

            // userEntity.Reservations = _userHelper.GetUserReservations(userEntity.Email);
            var userResponse = _mapper.Map<UserResponseModel>(userEntity);

            return userResponse;
        }
    }
}
