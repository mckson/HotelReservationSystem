using System;
using System.Collections.Generic;

namespace HotelReservation.Business.Models
{
    public class HotelModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int NumberFloors { get; set; }

        public double Deposit { get; set; }

        public string Description { get; set; }

        public HotelImageModel MainImage { get; set; }

        public IEnumerable<HotelImageModel> Images { get; set; }

        public LocationModel Location { get; set; }

        public IEnumerable<RoomModel> Rooms { get; set; }

        public List<HotelUserModel> HotelUsers { get; set; }

        public IEnumerable<ReservationModel> Reservations { get; set; }

        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
