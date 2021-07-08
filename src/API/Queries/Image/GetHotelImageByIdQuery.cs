using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HotelReservation.API.Queries.Image
{
    public class GetHotelImageByIdQuery : IRequest<FileContentResult>
    {
        public Guid Id { get; set; }
    }
}
