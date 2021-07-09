using FluentValidation;
using HotelReservation.API.Commands.Image;

namespace HotelReservation.API.Validation.Image
{
    public class CreateHotelImageCommandValidator : AbstractValidator<CreateHotelImageCommand>
    {
        public CreateHotelImageCommandValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Image must be not empty ({PropertyName})");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Name must be not empty ({PropertyName})");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("Type must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Type must be not empty ({PropertyName})");

            RuleFor(x => x.HotelId)
                .NotNull().WithMessage("Hotel Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Hotel Id must be not empty ({PropertyName})");
        }
    }
}
