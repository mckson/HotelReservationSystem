using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class BasePagedResponseModel<TResponseModel>
        where TResponseModel : class
    {
        public BasePagedResponseModel(
            IEnumerable<TResponseModel> data,
            int pageNumber,
            int pageSize)
        {
            Content = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public IEnumerable<TResponseModel> Content { get; set; }

        // Current page number
        public int PageNumber { get; set; }

        // Amount of content units per page
        public int PageSize { get; set; }

        public int TotalResults { get; set; }
    }
}
