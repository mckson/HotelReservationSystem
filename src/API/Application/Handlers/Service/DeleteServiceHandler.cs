using AutoMapper;
using HotelReservation.API.Application.Commands.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IManagementPermissionSupervisor _supervisor;
        private readonly IMapper _mapper;

        public DeleteServiceHandler(
            IServiceRepository serviceRepository,
            IManagementPermissionSupervisor supervisor,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _supervisor = supervisor;
            _mapper = mapper;
        }

        public async Task<ServiceResponseModel> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
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

            return deletedServiceResponse;
        }
    }
}
