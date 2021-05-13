using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class RoomModel
    {
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int FloorNumber { get; set; }

        public int Capacity { get; set; }

        public bool IsEmpty { get; set; }

        public int HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public IEnumerable<ReservationRoomModel> ReservationRoom { get; set; }
    }
}
