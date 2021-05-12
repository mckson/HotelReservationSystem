using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;

namespace HotelReservation.API.Models.RequestModels
{
    public class ReservationRequestModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public IEnumerable<int> RoomIds { get; set; }

        public DateTime ReservedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        [Required]
        public DateTime DateIn { get; set; }

        [Required]
        public DateTime DateOut { get; set; }

        public int TotalDays { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Deposit { get; set; }

        public double TotalPrice { get; set; }
    }
}
