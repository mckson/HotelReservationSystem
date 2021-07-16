using HotelReservation.API.Application.Commands.Image;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class UpdateHotelImageToMainHandler : IRequestHandler<UpdateHotelImageToMainCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IHotelImageHelper _hotelImageHelper;
        private readonly IManagementPermissionSupervisor _supervisor;

        public UpdateHotelImageToMainHandler(
            IHotelImageRepository hotelImageRepository,
            IManagementPermissionSupervisor supervisor,
            IHotelImageHelper hotelImageHelper)
        {
            _hotelImageRepository = hotelImageRepository;
            _supervisor = supervisor;
            _hotelImageHelper = hotelImageHelper;
        }

        public async Task<Unit> Handle(UpdateHotelImageToMainCommand request, CancellationToken cancellationToken)
        {
            var imageEntity = await _hotelImageRepository.GetAsync(request.Id) ??
                              throw new BusinessException("No image with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(imageEntity.HotelId);
            await _hotelImageHelper.ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            imageEntity.IsMain = true;

            await _hotelImageRepository.UpdateAsync(imageEntity);

            return Unit.Value;
        }
    }
}
