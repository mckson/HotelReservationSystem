using AutoMapper;
using HotelReservation.API.Application.Queries.Hotel;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Filters;
using HotelReservation.Data.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotelReservation.API.Application.Handlers.Hotel
{
    public class GetHotelSearchVariantsHandler : IRequestHandler<GetHotelSearchVariantsQuery, IEnumerable<HotelPromptResponseModel>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetHotelSearchVariantsHandler(
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotelPromptResponseModel>> Handle(GetHotelSearchVariantsQuery request, CancellationToken cancellationToken)
        {
            var paginationFilter = new PaginationFilter(
                PaginationValuesForSearchVariants.PageNumber,
                PaginationValuesForSearchVariants.PageSize);

            var hotelFilterExpression =
                FilterExpressions.GetHotelFilterExpression(request.HotelsFilter);

            var searchVariants =
                await Task.FromResult(_hotelRepository.Find(hotelFilterExpression, paginationFilter));

            var searchVariantsResponses = _mapper.Map<IEnumerable<HotelPromptResponseModel>>(searchVariants);

            return searchVariantsResponses;
        }
    }
}
