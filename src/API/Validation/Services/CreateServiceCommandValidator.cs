﻿using FluentValidation;
using HotelReservation.API.Commands.Service;

namespace HotelReservation.API.Validation.Services
{
    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Name must be not empty ({PropertyName})");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Price must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Price must be not null ({PropertyName})")
                .GreaterThan(0).WithMessage("Price must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(double.MaxValue).WithMessage($"Price must be less than or equal to {double.MaxValue} ({{PropertyName}})");

            RuleFor(x => x.HotelId)
                .NotNull().WithMessage("Smoking must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Smoking must be not null ({PropertyName})");
        }
    }
}
