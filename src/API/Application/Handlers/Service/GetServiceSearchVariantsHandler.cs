using AutoMapper;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class GetServiceSearchVariantsHandler : IRequestHandler<GetServiceSearchVariantsQuery, IEnumerable<ServicePromptResponseModel>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceSearchVariantsHandler(
            IServiceRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicePromptResponseModel>> Handle(GetServiceSearchVariantsQuery request, CancellationToken cancellationToken)
        {
            var serviceFilterExpression = FilterExpressions.GetServiceFilterExpression(request.ServicesFilter);
            var paginationFilter = new PaginationFilter(
                PaginationValuesForSearchVariants.PageNumber,
                PaginationValuesForSearchVariants.PageSize);

            var searchVariants =
                await Task.FromResult(_serviceRepository.Find(serviceFilterExpression, paginationFilter));

            var searchVariantsResponses = _mapper.Map<IEnumerable<ServicePromptResponseModel>>(searchVariants);

            return searchVariantsResponses;
        }
    }
}
