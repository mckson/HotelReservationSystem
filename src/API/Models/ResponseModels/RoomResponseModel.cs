using System.Collections.Generic;

namespace HotelReservation.API.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public double Price { get; set; }

        public int HotelId { get; set; }

        public IEnumerable<int> Reservations { get; set; }
    }
}
