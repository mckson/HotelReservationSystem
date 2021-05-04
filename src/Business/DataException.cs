using System;

namespace HotelReservation.Business
{
    public enum ErrorStatus
    {
        NotFound,
        HasLinkedEntity
    }

    public class DataException : Exception
    {
        public DataException()
            : base()
        {
            // empty ctor
        }

        public DataException(string message, ErrorStatus status)
            : base(message)
        {
            Status = status;
        }

        public ErrorStatus Status { get; set; }
    }
}
