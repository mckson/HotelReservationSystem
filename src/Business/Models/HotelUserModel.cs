using System;

namespace HotelReservation.Business.Models
{
    public class HotelUserModel
    {
        public Guid HotelId { get; set; }

        public HotelModel Hotel { get; set; }

        public Guid UserId { get; set; }

        public UserModel User { get; set; }
    }
}
