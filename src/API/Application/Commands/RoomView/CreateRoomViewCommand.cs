using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Application.Commands.RoomView
{
    public class CreateRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        public string Name { get; set; }
    }
}
