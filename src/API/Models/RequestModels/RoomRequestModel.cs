﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HotelReservation.API.Models.RequestModels
{
    public class RoomRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 10000)]
        public int RoomNumber { get; set; }

        [Required]
        [Range(1, 500)]
        public int FloorNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int Capacity { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Area { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public bool Smoking { get; set; }

        [Required]
        public bool Parking { get; set; }

        [Required]
        public string HotelId { get; set; }

        public IEnumerable<string> Facilities { get; set; }

        public IEnumerable<string> Views { get; set; }
    }
}
