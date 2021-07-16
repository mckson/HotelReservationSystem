using AutoMapper;
using HotelReservation.API.Application.Commands.Image;
using HotelReservation.API.Application.Interfaces;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Image
{
    public class CreateHotelImageHandler : IRequestHandler<CreateHotelImageCommand>
    {
        private readonly IHotelImageRepository _hotelImageRepository;
        private readonly IHotelImageHelper _hotelImageHelper;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;

        public CreateHotelImageHandler(
            IMapper mapper,
            IHotelImageRepository hotelImageRepository,
            IManagementPermissionSupervisor supervisor,
            IHotelImageHelper hotelImageHelper)
        {
            _mapper = mapper;
            _hotelImageRepository = hotelImageRepository;
            _supervisor = supervisor;
            _hotelImageHelper = hotelImageHelper;
        }

        public async Task<Unit> Handle(CreateHotelImageCommand request, CancellationToken cancellationToken)
        {
            var hotelId = Guid.Parse(request.HotelId);
            await _supervisor.CheckHotelManagementPermissionAsync(hotelId);

            var imageEntity = _mapper.Map<HotelImageEntity>(request);

            if (imageEntity.IsMain)
            {
                await _hotelImageHelper.ChangeHotelMainImageAsync(imageEntity.HotelId, imageEntity);
            }

            var createdImageEntity = await _hotelImageRepository.CreateAsync(imageEntity);

            return Unit.Value;
        }
    }
}
