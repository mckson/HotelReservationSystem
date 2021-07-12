using AutoMapper;
using HotelReservation.API.Application.Helpers;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetPagedFilteredHotelsHandler : IRequestHandler<GetPagedFilteredHotelsQuery, BasePagedResponseModel<HotelResponseModel>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public GetPagedFilteredHotelsHandler(
            IHotelRepository hotelRepository,
            IMapper mapper,
            IUriService uriService)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
            _uriService = uriService;
        }

        public async Task<BasePagedResponseModel<HotelResponseModel>> Handle(GetPagedFilteredHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotelFilterExpression = FilterExpressions.GetHotelFilterExpression(request.HotelsFilter);
            var countOfFilteredHotels = await _hotelRepository.GetCountAsync(hotelFilterExpression);

            request.PaginationFilter.PageSize ??= countOfFilteredHotels;
            var validPaginationFilter = new PaginationFilter(request.PaginationFilter.PageNumber, request.PaginationFilter.PageSize.Value);

            var hotelEntities = _hotelRepository.Find(
                hotelFilterExpression,
                validPaginationFilter);
            var hotelResponses = _mapper.Map<IEnumerable<HotelResponseModel>>(hotelEntities);

            var pagedResponse = PaginationHelper.CreatePagedResponseModel(
                hotelResponses,
                validPaginationFilter,
                countOfFilteredHotels,
                _uriService,
                request.Route);

            // var hotelModelsList = hotelModels.ToList();
            // foreach (var hotelModel in hotelModelsList)
            // {
            //     foreach (var hotelUser in hotelModel.HotelUsers)
            //     {
            //         await GetRolesForUserModelAsync(hotelUser.User);
            //     }
            // }
            return pagedResponse;
        }
    }
}
