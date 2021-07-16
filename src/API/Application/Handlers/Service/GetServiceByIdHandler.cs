using AutoMapper;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class GetServiceByIdHandler : IRequestHandler<GetServiceByIdQuery, ServiceResponseModel>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceByIdHandler(
            IServiceRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponseModel> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceEntity = await _serviceRepository.GetAsync(request.Id) ??
                                throw new BusinessException(
                                    "Service with such id does not exist",
                                    ErrorStatus.NotFound);

            var serviceResponse = _mapper.Map<ServiceResponseModel>(serviceEntity);

            return serviceResponse;
        }
    }
}
