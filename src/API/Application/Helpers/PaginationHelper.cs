using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Data.Filters;
using System.Collections.Generic;

namespace HotelReservation.API.Application.Helpers
{
    public class PaginationHelper
    {
        public static BasePagedResponseModel<TResponseModel> CreatePagedResponseModel<TResponseModel>(
            IEnumerable<TResponseModel> content,
            PaginationFilter filter,
            int totalResults)
            where TResponseModel : class
        {
            var response = new BasePagedResponseModel<TResponseModel>(content, filter.PageNumber, filter.PageSize)
            {
                TotalResults = totalResults
            };

            return response;
        }
    }
}
