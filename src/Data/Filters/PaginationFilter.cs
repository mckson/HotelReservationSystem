namespace HotelReservation.Data.Filters
{
    public class PaginationFilter
    {
        public const int MinPageNumber = 1;
        public const int MinPageSize = 5;
        // public const int MaxPageSize = 50;
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;

        public PaginationFilter()
        {
            PageNumber = DefaultPageNumber;
            PageSize = DefaultPageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < MinPageNumber ? MinPageNumber : pageNumber;

            if (pageSize < MinPageSize)
                pageSize = MinPageSize;
            // else if (pageSize > MaxPageSize)
            //    pageSize = MaxPageSize;
            PageSize = pageSize;
        }

        /// <summary>
        /// Number of required page (offset from start of collection of items)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Size of page - limitation in amount of items per one page
        /// </summary>
        public int? PageSize { get; set; }
    }
}
