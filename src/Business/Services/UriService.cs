using HotelReservation.Business.Interfaces;
using HotelReservation.Data.Filters;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace HotelReservation.Business.Services
{
    public class UriService : IUriService
    {
        private const string PageNumber = "pageNumber";
        private const string PageSize = "PageSize";

        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var endpointUri = string.Concat(_baseUri, route);
            endpointUri = QueryHelpers.AddQueryString(endpointUri.ToString(), PageNumber, filter.PageNumber.ToString());
            endpointUri = QueryHelpers.AddQueryString(endpointUri.ToString(), PageSize, filter.PageSize.ToString());

            return new Uri(endpointUri);
        }
    }
}
