using AutoMapper;
using HotelReservation.API.Application.Queries.User;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserBriefResponseModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserBriefResponseModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var userEntities = await _userRepository.GetAll();
            var userResponses = _mapper.Map<IEnumerable<UserBriefResponseModel>>(userEntities);

            return userResponses;
        }
    }
}
