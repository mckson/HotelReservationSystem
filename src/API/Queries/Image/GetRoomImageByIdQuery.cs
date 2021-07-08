using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HotelReservation.API.Queries.Image
{
    public class GetRoomImageByIdQuery : IRequest<FileContentResult>
    {
        public Guid Id { get; set; }
    }
}
