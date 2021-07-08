using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.RoomView
{
    public class UpdateRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
