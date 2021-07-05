﻿using System;

namespace HotelReservation.Business.Models
{
    public class HotelImageModel
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsMain { get; set; }

        public Guid HotelId { get; set; }

        public virtual HotelModel Hotel { get; set; }
    }
}
