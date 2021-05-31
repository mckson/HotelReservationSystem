using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using System;
using System.Collections.Generic;
using HotelReservation.Data.Filters;

namespace HotelReservation.Business.Helpers
{
    public class PaginationHelper
    {
        public static BasePagedResponseModel<TResponseModel> CreatePagedResponseModel<TResponseModel>(
            IEnumerable<TResponseModel> content,
            PaginationFilter filter,
            int totalResults,
            IUriService uriService,
            string route)
            where TResponseModel : class
        {
            var response = new BasePagedResponseModel<TResponseModel>(content, filter.PageNumber, filter.PageSize);
            var totalPages = Convert.ToInt32(Math.Ceiling((double)totalResults / (double)filter.PageSize));

            response.NextPage = filter.PageNumber >= 1 && filter.PageNumber < totalPages
                ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber + 1, filter.PageSize), route)
                : null;

            response.PreviousPage = filter.PageNumber - 1 >= 1 && filter.PageNumber <= totalPages
                ? uriService.GetPageUri(new PaginationFilter(filter.PageNumber - 1, filter.PageSize), route)
                : null;

            response.FirstPage = uriService.GetPageUri(new PaginationFilter(1, filter.PageSize), route);

            response.LastPage = uriService.GetPageUri(new PaginationFilter(totalPages, filter.PageSize), route);

            response.TotalPages = totalPages;

            response.TotalResults = totalResults;

            return response;
        }
    }
}
