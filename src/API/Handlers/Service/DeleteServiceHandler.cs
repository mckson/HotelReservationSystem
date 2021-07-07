using AutoMapper;
using HotelReservation.API.Commands.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Handlers.Service
{
    public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeleteServiceHandler(
            IServiceRepository serviceRepository,
            IManagementPermissionSupervisor supervisor,
            IMapper mapper,
            ILogger logger)
        {
            _serviceRepository = serviceRepository;
            _supervisor = supervisor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponseModel> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Service {request.Id} is deleting");

            // was as no tracking
            var serviceEntity = await _serviceRepository.GetAsync(request.Id) ??
                                throw new BusinessException("No service with such id", ErrorStatus.NotFound);

            if (serviceEntity.HotelId != null)
            {
                await _supervisor.CheckHotelManagementPermissionAsync(serviceEntity.HotelId.Value);
            }
            else
            {
                throw new BusinessException(
                    "Hotel id was null. Room cannot be created without hotel",
                    ErrorStatus.NotFound);
            }

            var deletedServiceEntity = await _serviceRepository.DeleteAsync(request.Id);
            var deletedServiceResponse = _mapper.Map<ServiceResponseModel>(deletedServiceEntity);

            _logger.Debug($"Service {request.Id} deleted");

            return deletedServiceResponse;
        }
    }
}
