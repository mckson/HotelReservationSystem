using System;
using FluentValidation;
using HotelReservation.API.Application.Commands.Reservation;

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
                .NotNull().WithMessage("Date in must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Date in must be not empty ({PropertyName})")
                .Must(dateIn => dateIn >= DateTime.Now).WithMessage("Date in must be today or later ({PropertyName})");

            RuleFor(x => x.DateOut)
                .NotNull().WithMessage("Date out must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Date out must be not empty ({PropertyName})")
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
        }
    }
}
