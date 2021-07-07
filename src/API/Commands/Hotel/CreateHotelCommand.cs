using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Commands.Hotel
{
    public class CreateHotelCommand : IRequest<HotelResponseModel>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(1, 500)]
        public int NumberFloors { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Deposit { get; set; }

        [Required]
        [MaxLength(10000)]
        public string Description { get; set; }

        [Required]
        public LocationRequestModel Location { get; set; }

        public IEnumerable<Guid> Managers { get; set; }
    }
}
