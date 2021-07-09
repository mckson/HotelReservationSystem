using System.Collections.Generic;
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
    public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<ServiceResponseModel>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetAllServicesHandler(
            IServiceRepository serviceRepository,
            IMapper mapper,
            ILogger logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceResponseModel>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug("Services are requesting");

            var serviceEntities = await Task.FromResult(_serviceRepository.GetAll()) ??
                                  throw new BusinessException("No services were created", ErrorStatus.NotFound);

            var serviceResponses = _mapper.Map<IEnumerable<ServiceResponseModel>>(serviceEntities);

            _logger.Debug("Services requested");

            return serviceResponses;
        }
    }
}
