using HotelReservation.API.Application.Commands.Room;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.API.Options;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HotelReservation.API.Application.Handlers.Room
{
    public class LockRoomHandler : IRequestHandler<LockRoomCommand>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly LockRoomOptions _lockRoomOptions;

        public LockRoomHandler(
            IRoomRepository roomRepository,
            IOptions<LockRoomOptions> lockRoomOptions,
            IAuthenticationHelper authenticationHelper)
        {
            _roomRepository = roomRepository;
            _lockRoomOptions = lockRoomOptions.Value;
            _authenticationHelper = authenticationHelper;
        }

        public async Task<Unit> Handle(LockRoomCommand request, CancellationToken cancellationToken)
        {
            var roomEntity = await _roomRepository.GetAsync(request.Id) ??
                             throw new BusinessException("Room does not exist", ErrorStatus.NotFound);

            var userId = _authenticationHelper.GetCurrentUserId();

            var isLocked = DateTime.UtcNow < roomEntity.UnlockTime;
            var isCurrentUserAuthorized = userId.HasValue;
            var isRoomLockedByAuthorizedUser = roomEntity.LockedByUserId.HasValue;
            var isCurrentUserAndUserLockedRoomSame = isCurrentUserAuthorized && isRoomLockedByAuthorizedUser && userId.Value.Equals(roomEntity.LockedByUserId);

            if (isLocked && ((!isCurrentUserAuthorized && !isRoomLockedByAuthorizedUser) ||
                             (!isCurrentUserAndUserLockedRoomSame)))
            {
                throw new BusinessException("Room is already locked by another user", ErrorStatus.AccessDenied);
            }

            var unlockTime = DateTime.UtcNow + (userId.HasValue
                ? TimeSpan.FromMinutes(_lockRoomOptions.LockTimeInMinutes)
                : TimeSpan.FromMinutes(_lockRoomOptions.UnauthorizedLockTimeInMinutes));

            roomEntity.UnlockTime = unlockTime;
            roomEntity.LockedByUserId = userId;

            await _roomRepository.UpdateAsync(roomEntity);

            return Unit.Value;
        }
    }
}
