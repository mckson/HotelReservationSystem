using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.RoomView
{
    public class CreateRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        [Required]
        public string Name { get; set; }
    }
}
