using System;
using System.Globalization;
using static System.String;

namespace HotelReservation.Business
{
    public enum ErrorStatus
    {
        NotFound,
        EmptyInput,
        IncorrectInput,
        AlreadyExist,
        AccessDenied
    }

    public class BusinessException : Exception
    {
        public BusinessException()
            : base()
        {
            // empty ctor
        }

        public BusinessException(string message, ErrorStatus status)
            : base(message)
        {
            Status = status;
        }

        public BusinessException(string message, ErrorStatus status, params object[] args)
            : base(Format(CultureInfo.CurrentCulture, message, args))
        {
            Status = status;
        }

        public ErrorStatus Status { get; set; }
    }
}
