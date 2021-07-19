using FluentValidation;
using HotelReservation.API.Application.Commands.Reservation;
using System;
using System.Text.RegularExpressions;

namespace HotelReservation.API.Application.Validation.Reservation
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.HotelId)
                .NotNull().WithMessage("Hotel Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Hotel Id must be not empty ({PropertyName})");

            RuleFor(x => x.Rooms)
                .NotNull().WithMessage("Rooms must be not null ({PropertyName})");

            RuleFor(x => x.Services)
                .NotNull().WithMessage("Services must be not null ({PropertyName})");

            RuleFor(x => x.DateIn)
                .Must(dateIn => dateIn >= DateTime.UtcNow.Date).WithMessage("Date in must be today or later ({PropertyName})");

            RuleFor(x => x.DateOut)
                .GreaterThan(x => x.DateIn).WithMessage("Date out must be later than date in ({PropertyName})");

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("Surname must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Surname must be not empty ({PropertyName})");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("Name must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Name must be not empty ({PropertyName})");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Email must be not empty ({PropertyName})")
                .EmailAddress().WithMessage("Input value {PropertyValue} must be valid email address {PropertyName}");

            RuleFor(x => x.PassportNumber)
                .NotNull().WithMessage(
                    "Passport number is required, because your order will be identified by passport number ({PropertyName})")
                .NotEmpty().WithMessage(
                    "Passport number is required, because your order will be identified by passport number  ({PropertyName})");

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage(
                    "Phone number is required, because managers may need way to connect with you ({PropertyName})")
                .NotEmpty().WithMessage(
                    "Phone number is required, because managers may need way to connect with you ({PropertyName})")
                .Must(number => number != null && Regex.IsMatch(
                    number,
                    @"^\+?\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"))
                .WithMessage("Input value {PropertyValue} must be phone number ({PropertyName})");
        }
    }
}
