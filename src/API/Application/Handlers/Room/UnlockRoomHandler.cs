using System;
using HotelReservation.API.Application.Commands.Room;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class UnlockRoomHandler : IRequestHandler<UnlockRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAuthenticationHelper _authenticationHelper;

        public UnlockRoomHandler(IRoomRepository roomRepository, IAuthenticationHelper authenticationHelper)
        {
            _roomRepository = roomRepository;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<Unit> Handle(UnlockRoomCommand request, CancellationToken cancellationToken)
        {
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("Room does not exist", ErrorStatus.NotFound);

            var userId = _authenticationHelper.GetCurrentUserId();

            var isLocked = roomEntity.UnlockTime.HasValue && DateTime.UtcNow < roomEntity.UnlockTime;

            if (!isLocked)
            {
                throw new BusinessException("Room is already unlocked", ErrorStatus.NotFound);
            }

            if ((userId.HasValue && !userId.Value.Equals(roomEntity.LockedByUserId)) ||
                (!userId.HasValue && roomEntity.LockedByUserId.HasValue))
            {
                throw new BusinessException("You cannot unlock room locked by another user", ErrorStatus.AccessDenied);
            }

            roomEntity.LockedByUserId = null;
            roomEntity.UnlockTime = null;

            await _roomRepository.UpdateAsync(roomEntity);

            return Unit.Value;
        }
    }
}
