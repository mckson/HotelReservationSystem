using AutoMapper;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.API.Queries.User;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.User
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserBriefResponseModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IUserRepository userRepository, ILogger logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserBriefResponseModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug("Users are requesting");

            var userEntities = await _userRepository.GetAll();
            var userResponses = _mapper.Map<IEnumerable<UserBriefResponseModel>>(userEntities);

            _logger.Debug("Users requested");

            return userResponses;
        }
    }
}
