using System.Collections.Generic;
using HotelReservation.Business.Models.UserModels;

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

        public virtual HotelModel Hotel { get; set; }

        public virtual ReservationModel Reservation { get; set; }

        public virtual IEnumerable<UserModel> Users { get; set; }
    }
}
