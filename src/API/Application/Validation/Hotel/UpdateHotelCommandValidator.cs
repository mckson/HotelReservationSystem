using FluentValidation;
using HotelReservation.API.Application.Commands.Hotel;

namespace HotelReservation.API.Application.Validation.Hotel
{
    public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
    {
        public UpdateHotelCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Id must be not empty ({PropertyName})");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Name must be not empty ({PropertyName})")
                .MaximumLength(100).WithMessage("Name must be 100 characters or less ({PropertyName})");

            RuleFor(x => x.NumberFloors)
                .NotNull().WithMessage("Number floors must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Number floors must be not empty ({PropertyName})")
                .Must(number => number > 1 && number < 500)
                .WithMessage("Number floors must be 1 or more, but less or equal 500 ({PropertyName})");

            RuleFor(x => x.Deposit)
                .NotNull().WithMessage("Deposit must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Deposit must be not empty ({PropertyName})")
                .Must(deposit => deposit > 0 && deposit < double.MaxValue)
                .WithMessage("Deposit must be 0 or more ({PropertyName})");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("Description must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Description must be not empty ({PropertyName})")
                .MaximumLength(10000).WithMessage("Description must be 10000 or less ({PropertyName})");

            RuleFor(x => x.Location)
                .NotNull().WithMessage("Location must be not null ({PropertyName})");

            RuleFor(x => x.Location.Country)
                .NotNull().WithMessage("Country must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Country must be not empty ({PropertyName})")
                .MaximumLength(50).WithMessage("Country must be 50 or less ({PropertyName})")
                .When(x => x.Location != null);

            RuleFor(x => x.Location.Region)
                .NotNull().WithMessage("Region must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Region must be not empty ({PropertyName})")
                .MaximumLength(50).WithMessage("Region must be 50 or less ({PropertyName})")
                .When(x => x.Location != null);

            RuleFor(x => x.Location.City)
                .NotNull().WithMessage("City must be not null ({PropertyName})")
                .NotEmpty().WithMessage("City must be not empty ({PropertyName})")
                .MaximumLength(50).WithMessage("City must be 50 or less ({PropertyName})")
                .When(x => x.Location != null);

            RuleFor(x => x.Location.Street)
                .NotNull().WithMessage("Street must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Street must be not empty ({PropertyName})")
                .MaximumLength(100).WithMessage("Street must be 100 or less ({PropertyName})")
                .When(x => x.Location != null);

            RuleFor(x => x.Location.BuildingNumber)
                .NotNull().WithMessage("Building number must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Building number must be not empty ({PropertyName})")
                .Must(number => number > 1 && number < 1000)
                .WithMessage("Building number must be 1000 or less, but more than 0 ({PropertyName})")
                .When(x => x.Location != null);
        }
    }
}
