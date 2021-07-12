using HotelReservation.API.Application.Commands.User;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.User
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;

        public DeleteUserHandler(
            IUserRepository userRepository,
            IUserHelper userHelper)
        {
            _userRepository = userRepository;
            _userHelper = userHelper;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var isCurrentUser = _userHelper.IsCurrentUser(request.Id);

            if (isCurrentUser)
            {
                throw new BusinessException("You cannot delete yourself", ErrorStatus.IncorrectInput);
            }

            // await GetRolesForUserModelAsync(deletedUserModel);
            var result = await _userRepository.DeleteAsync(request.Id);

            if (!result)
            {
                throw new BusinessException("No user with such id", ErrorStatus.NotFound);
            }

            return Unit.Value;
        }
    }
}
