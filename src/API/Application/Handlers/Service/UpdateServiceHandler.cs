using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Commands.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class UpdateServiceHandler : IRequestHandler<UpdateServiceCommand, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly IManagementPermissionSupervisor _supervisor;

        public UpdateServiceHandler(
            IServiceRepository serviceRepository,
            IHotelRepository hotelRepository,
            IManagementPermissionSupervisor supervisor,
            ILogger logger,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponseModel> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Service {request.Id} is updating");

            // was as no tracking
            var serviceEntity = await _serviceRepository.GetAsync(request.Id) ??
                             throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            if (serviceEntity.HotelId != null)
            {
                var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId.Value) ??
                                  throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

                await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id);

                // was as no tracking
                if (_serviceRepository.GetAll().Any(service =>
                    string.Equals(service.Name, request.Name, StringComparison.CurrentCultureIgnoreCase) &&
                    service.HotelId == serviceEntity.HotelId &&
                    serviceEntity.Id != service.Id))
                {
                    throw new BusinessException(
                        $"Service with such name already exist in {hotelEntity.Name}",
                        ErrorStatus.AlreadyExist);
                }
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            serviceEntity.Name = request.Name;
            serviceEntity.Price = request.Price;

            ServiceEntity updatedServiceEntity;
            try
            {
                updatedServiceEntity = await _serviceRepository.UpdateAsync(serviceEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedServiceResponse = _mapper.Map<ServiceResponseModel>(updatedServiceEntity);

            _logger.Debug($"Service {request.Id} updated");

            return updatedServiceResponse;
        }
    }
}
