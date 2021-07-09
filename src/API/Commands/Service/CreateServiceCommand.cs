using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Commands.Service
{
    public class CreateServiceCommand : IRequest<ServiceResponseModel>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string HotelId { get; set; }
    }
}
