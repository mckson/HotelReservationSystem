using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.Service;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Service
{
    public class GetPagedFilteredServicesHandler : IRequestHandler<GetPagedFilteredServicesQuery, BasePagedResponseModel<ServiceResponseModel>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetPagedFilteredServicesHandler(
            IServiceRepository serviceRepository,
            IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<BasePagedResponseModel<ServiceResponseModel>> Handle(GetPagedFilteredServicesQuery request, CancellationToken cancellationToken)
        {
            var servicesFilterExpression = FilterExpressions.GetServiceFilterExpression(request.ServicesFilter);
            var countFilteredServices = await _serviceRepository.GetCountAsync(servicesFilterExpression);
            var validPaginationFilter =
                new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);

            var serviceEntities = _serviceRepository.Find(servicesFilterExpression, validPaginationFilter);

            var serviceResponses = _mapper.Map<IEnumerable<ServiceResponseModel>>(serviceEntities);

            var pagedResponse =
                PaginationHelper.CreatePagedResponseModel(
                    serviceResponses,
                    validPaginationFilter,
                    countFilteredServices);

            return pagedResponse;
        }
    }
}
