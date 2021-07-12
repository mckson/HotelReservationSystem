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
        private readonly IUserHelper _userHelper;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        public async Task<UserResponseModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByIdAsync(request.Id) ??
                             throw new BusinessException($"User with id {request.Id} does not exist", ErrorStatus.NotFound);

            userEntity.Reservations = _userHelper.GetUserReservations(userEntity.Email);

            var userResponse = _mapper.Map<UserResponseModel>(userEntity);

            return userResponse;
        }
    }
}
