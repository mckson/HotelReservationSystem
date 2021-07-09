using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.Reservation;
using HotelReservation.API.Helpers;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using Serilog;

namespace HotelReservation.API.Application.Handlers.Reservation
{
    public class GetPagedFilteredReservationsHandler : IRequestHandler<GetPagedFilteredReservationsQuery, BasePagedResponseModel<ReservationBriefResponseModel>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUriService _uriService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetPagedFilteredReservationsHandler(
            IReservationRepository reservationRepository,
            ILogger logger,
            IMapper mapper,
            IUriService uriService)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<BasePagedResponseModel<ReservationBriefResponseModel>> Handle(GetPagedFilteredReservationsQuery request, CancellationToken cancellationToken)
        {
            _logger.Debug(
                "Paged reservations for user " +
                $"{request.ReservationsFilter.Email} are requesting. , " +
                $"page: {request.PaginationFilter.PageNumber}, size: {request.PaginationFilter.PageSize}");

            var reservationsFilter = FilterExpressions.GetReservationFilterExpression(request.ReservationsFilter);
            var countOfFilteredReservations = await _reservationRepository.GetCountAsync(reservationsFilter);

            request.PaginationFilter.PageSize ??= countOfFilteredReservations;
            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize.Value);

            var reservationEntities =
                _reservationRepository.Find(reservationsFilter, validPaginationFilter);

            var reservationResponses = _mapper.Map<IEnumerable<ReservationBriefResponseModel>>(reservationEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                reservationResponses,
                validPaginationFilter,
                countOfFilteredReservations,
                _uriService,
                request.Route);

            _logger.Debug(
                "Paged reservations for user " +
                $"{request.ReservationsFilter.Email} are requested. , " +
                $"page: {request.PaginationFilter.PageNumber}, size: {request.PaginationFilter.PageSize}");

            return pagedResponse;
        }
    }
}
