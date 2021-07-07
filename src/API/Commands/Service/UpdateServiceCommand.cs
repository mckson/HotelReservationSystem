using HotelReservation.API.Models.ResponseModels;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.API.Commands.Service
{
    public class UpdateServiceCommand : IRequest<ServiceResponseModel>
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public string HotelId { get; set; }
    }
}
