using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetServiceByIdHandler(IServiceRepository serviceRepository, IMapper mapper, ILogger logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponseModel> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug($"Service {request.Id} is requesting");

            var serviceEntity = await _serviceRepository.GetAsync(request.Id) ??
                                throw new BusinessException(
                                    "Service with such id does not exist",
                                    ErrorStatus.NotFound);

            var serviceResponse = _mapper.Map<ServiceResponseModel>(serviceEntity);

            _logger.Debug($"Service {request.Id} requested");

            return serviceResponse;
        }
    }
}
