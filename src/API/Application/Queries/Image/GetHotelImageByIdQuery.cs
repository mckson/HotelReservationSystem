using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.API.Application.Queries.Image
{
    public class GetHotelImageByIdQuery : IRequest<FileContentResult>
    {
        public Guid Id { get; set; }
    }
}
