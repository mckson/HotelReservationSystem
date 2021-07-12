using AutoMapper;
using HotelReservation.API.Application.Commands.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class CreateServiceHandler : IRequestHandler<CreateServiceCommand, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;

        public CreateServiceHandler(
            IServiceRepository serviceRepository,
            IMapper mapper,
            IHotelRepository hotelRepository,
            IManagementPermissionSupervisor supervisor)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
        }

        public async Task<ServiceResponseModel> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            var serviceEntity = _mapper.Map<ServiceEntity>(request);

            if (serviceEntity.HotelId != null)
            {
                var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId.Value) ??
                                  throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

                await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id);

                if (hotelEntity.Services.Any(service => string.Equals(service.Name, serviceEntity.Name, StringComparison.CurrentCultureIgnoreCase)))
                    throw new BusinessException($"Service with such name already exist in {hotelEntity.Name}", ErrorStatus.AlreadyExist);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var createdServiceEntity = await _serviceRepository.CreateAsync(serviceEntity);
            var createdServiceResponse = _mapper.Map<ServiceResponseModel>(createdServiceEntity);

            return createdServiceResponse;
        }
    }
}
