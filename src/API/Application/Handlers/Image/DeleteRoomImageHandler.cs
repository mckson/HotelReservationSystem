using HotelReservation.API.Application.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class DeleteRoomImageHandler : IRequestHandler<DeleteRoomImageCommand>
    {
        private readonly IRoomImageRepository _roomImageRepository;

        public DeleteRoomImageHandler(IRoomImageRepository roomImageRepository)
        {
            _roomImageRepository = roomImageRepository;
        }

        public async Task<Unit> Handle(DeleteRoomImageCommand request, CancellationToken cancellationToken)
        {
            var imageEntity = await _roomImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            // await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await _roomImageRepository.DeleteAsync(imageEntity.Id);

            return Unit.Value;
        }
    }
}
