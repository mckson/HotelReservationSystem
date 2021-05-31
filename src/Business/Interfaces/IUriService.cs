using System;
using HotelReservation.Data.Filters;

namespace HotelReservation.Business.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
