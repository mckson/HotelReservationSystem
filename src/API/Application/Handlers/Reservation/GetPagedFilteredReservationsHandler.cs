using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.Reservation;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Reservation
{
    public class GetPagedFilteredReservationsHandler : IRequestHandler<GetPagedFilteredReservationsQuery, BasePagedResponseModel<ReservationBriefResponseModel>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;

        public GetPagedFilteredReservationsHandler(
            IReservationRepository reservationRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
        }

        public async Task<BasePagedResponseModel<ReservationBriefResponseModel>> Handle(GetPagedFilteredReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservationsFilter = FilterExpressions.GetReservationFilterExpression(request.ReservationsFilter);
            var countOfFilteredReservations = await _reservationRepository.GetCountAsync(reservationsFilter);

            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize);

            var reservationEntities =
                _reservationRepository.Find(reservationsFilter, validPaginationFilter);

            var reservationResponses = _mapper.Map<IEnumerable<ReservationBriefResponseModel>>(reservationEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                reservationResponses,
                validPaginationFilter,
                countOfFilteredReservations);

            return pagedResponse;
        }
    }
}
