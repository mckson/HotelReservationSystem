using System;

namespace HotelReservation.Business
{
    public enum ErrorStatus
    {
        NotFound,
        HasLinkedEntity,
        EmptyInput,
        IncorrectInput,
        AlreadyExisted
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
