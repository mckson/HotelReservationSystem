using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotelReservation.API.Models.ResponseModels;
using MediatR;

namespace HotelReservation.API.Commands.Reservation
{
    public class CreateReservationCommand : IRequest<ReservationBriefResponseModel>
    {
        [Required]
        public string HotelId { get; set; }

        [Required]
        public IEnumerable<string> Rooms { get; set; }

        [Required]
        public IEnumerable<string> Services { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2021", "1/1/2999")]
        public DateTime DateIn { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/2021", "1/1/2999")]
        public DateTime DateOut { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
