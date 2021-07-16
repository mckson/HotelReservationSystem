using HotelReservation.API.Application.Commands.Image;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class DeleteHotelImageHandler : IRequestHandler<DeleteHotelImageCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IManagementPermissionSupervisor _supervisor;

        public DeleteHotelImageHandler(
            IHotelImageRepository hotelImageRepository,
            IManagementPermissionSupervisor supervisor)
        {
            _hotelImageRepository = hotelImageRepository;
            _supervisor = supervisor;
        }

        public async Task<Unit> Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
        {
            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await _hotelImageRepository.DeleteAsync(request.Id);

            return Unit.Value;
        }
    }
}
