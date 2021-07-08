using AutoMapper;
using HotelReservation.API.Commands.User;
using HotelReservation.API.Interfaces;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.User
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DeleteUserHandler(IUserRepository userRepository, ILogger logger, IMapper mapper, IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _userHelper = userHelper;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"User {request.Id} is deleting");

            var isCurrentUser = _userHelper.IsCurrentUser(request.Id);

            if (isCurrentUser)
            {
                throw new BusinessException("You cannot delete yourself", ErrorStatus.IncorrectInput);
            }

            // await GetRolesForUserModelAsync(deletedUserModel);
            var result = await _userRepository.DeleteAsync(request.Id;

            if (!result)
            {
                throw new BusinessException("No user with such id", ErrorStatus.NotFound);
            }

            _logger.Debug($"User {request.Id} is deleted");

            return Unit.Value;
        }
    }
}
