using FluentValidation;
using HotelReservation.API.Application.Commands.Service;

namespace HotelReservation.API.Application.Validation.Services
{
    public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Id must be not empty ({PropertyName})");

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
