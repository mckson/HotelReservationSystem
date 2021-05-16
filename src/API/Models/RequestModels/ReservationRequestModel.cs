using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Models.RequestModels
{
    public class ReservationRequestModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int HotelId { get; set; }

        [Required]
        public IEnumerable<int> Rooms { get; set; }

        [Required]
        public IEnumerable<int> Services { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2021", "1/1/2999")]
        public DateTime DateIn { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2021", "1/1/2999")]
        public DateTime DateOut { get; set; }

        public int TotalDays { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Deposit { get; set; }

        public double TotalPrice { get; set; }
    }
}
