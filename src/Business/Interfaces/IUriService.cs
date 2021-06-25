using HotelReservation.Data.Filters;
using System;

namespace HotelReservation.Business.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(PaginationFilter filter, string route);

        Uri GetResourceUri(string route, string resourceId);
    }
}
