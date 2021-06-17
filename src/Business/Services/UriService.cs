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
            endpointUri = QueryHelpers.AddQueryString(endpointUri, PageNumber, filter.PageNumber.ToString());
            endpointUri = QueryHelpers.AddQueryString(endpointUri, PageSize, filter.PageSize.ToString());

            return new Uri(endpointUri);
        }

        public Uri GetResourceUri(string route, int resourceId)
        {
            var endpointUri = string.Concat(_baseUri, route);
            endpointUri = string.Concat(endpointUri, $"/{resourceId}");

            return new Uri(endpointUri);
        }
    }
}
