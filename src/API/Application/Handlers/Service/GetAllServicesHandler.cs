using AutoMapper;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, IEnumerable<ServiceResponseModel>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetAllServicesHandler(
            IServiceRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceResponseModel>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var serviceEntities = await Task.FromResult(_serviceRepository.GetAll()) ??
                                  throw new BusinessException("No services were created", ErrorStatus.NotFound);

            var serviceResponses = _mapper.Map<IEnumerable<ServiceResponseModel>>(serviceEntities);

            return serviceResponses;
        }
    }
}
