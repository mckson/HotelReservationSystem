using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Commands.RoomView
{
    public class CreateRoomViewCommand : IRequest<RoomViewResponseModel>
    {
        public string Name { get; set; }
    }
}
