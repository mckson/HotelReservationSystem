using AutoMapper;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ServiceEntity> _serviceRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ILogger _logger;
        private readonly ManagementPermissionSupervisor _supervisor;

        public ServicesService(
            IRepository<ServiceEntity> serviceRepository,
            IHotelRepository hotelRepository,
            ManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _serviceRepository = serviceRepository;
            _hotelRepository = hotelRepository;
            _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceModel> CreateAsync(ServiceModel serviceModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug("Service is creating");

            var serviceEntity = _mapper.Map<ServiceEntity>(serviceModel);

            var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId.Value) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            if (hotelEntity.Services.Any(service => string.Equals(service.Name, serviceEntity.Name, StringComparison.CurrentCultureIgnoreCase)))
                throw new BusinessException($"Service with such name already exist in {hotelEntity.Name}", ErrorStatus.AlreadyExist);

            var createdServiceEntity = await _serviceRepository.CreateAsync(serviceEntity);
            var createdServiceModel = _mapper.Map<ServiceModel>(createdServiceEntity);

            _logger.Debug($"Service {createdServiceModel.Id} created");

            return createdServiceModel;
        }

        public async Task<ServiceModel> GetAsync(int id)
        {
            _logger.Debug($"Service {id} is requesting");

            var serviceEntity = await _serviceRepository.GetAsync(id) ??
                                throw new BusinessException(
                                    "Service with such id does not exist",
                                    ErrorStatus.NotFound);

            var serviceModel = _mapper.Map<ServiceModel>(serviceEntity);

            _logger.Debug($"Service {id} requested");

            return serviceModel;
        }

        public async Task<ServiceModel> DeleteAsync(int id, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Service {id} is deleting");

            // was as no tracking
            var serviceEntity = await _serviceRepository.GetAsync(id) ??
                                throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(serviceEntity.HotelId.Value, userClaims);

            var deletedServiceEntity = await _serviceRepository.DeleteAsync(id);
            var deletedServiceModel = _mapper.Map<ServiceModel>(deletedServiceEntity);

            _logger.Debug($"Service {id} deleted");

            return deletedServiceModel;
        }

        public async Task<ServiceModel> UpdateAsync(int id, ServiceModel updatingServiceModel, IEnumerable<Claim> userClaims)
        {
            _logger.Debug($"Service {id} is updating");

            // was as no tracking
            var serviceEntity = await _serviceRepository.GetAsync(id) ??
                             throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            var hotelEntity = await _hotelRepository.GetAsync(serviceEntity.HotelId.Value) ??
                              throw new BusinessException("No hotel with such id", ErrorStatus.NotFound);

            await _supervisor.CheckHotelManagementPermissionAsync(hotelEntity.Id, userClaims);

            // was as no tracking
            if (_serviceRepository.GetAll().Any(service =>
                string.Equals(service.Name, updatingServiceModel.Name, StringComparison.CurrentCultureIgnoreCase) &&
                service.HotelId == serviceEntity.HotelId &&
                serviceEntity.Id != service.Id))
            {
                throw new BusinessException(
                    $"Service with such name already exist in {hotelEntity.Name}",
                    ErrorStatus.AlreadyExist);
            }

            // var updatingServiceEntity = _mapper.Map<ServiceEntity>(updatingServiceModel);
            serviceEntity.Name = updatingServiceModel.Name;
            serviceEntity.Price = updatingServiceModel.Price;

            ServiceEntity updatedServiceEntity;
            try
            {
                updatedServiceEntity = await _serviceRepository.UpdateAsync(serviceEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ErrorStatus.IncorrectInput);
            }

            var updatedServiceModel = _mapper.Map<ServiceModel>(updatedServiceEntity);

            _logger.Debug($"Service {id} updated");

            return updatedServiceModel;
        }

        public IEnumerable<ServiceModel> GetAllServices()
        {
            _logger.Debug("Services are requesting");

            var serviceEntities = _serviceRepository.GetAll() ??
                                  throw new BusinessException("No services were created", ErrorStatus.NotFound);

            var serviceModels = _mapper.Map<IEnumerable<ServiceModel>>(serviceEntities);

            _logger.Debug("Services requested");

            return serviceModels;
        }
    }
}
