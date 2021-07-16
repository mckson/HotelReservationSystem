using FluentValidation;
using HotelReservation.API.Application.Commands.Room;

namespace HotelReservation.API.Application.Validation.Room
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Name must be not empty ({PropertyName})");

            RuleFor(x => x.RoomNumber)
                .GreaterThan(0).WithMessage("Room number must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(10000).WithMessage("Room number must be less than or equal to 10000 ({PropertyName})");

            RuleFor(x => x.FloorNumber)
                .GreaterThan(0).WithMessage("Floor number must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(500).WithMessage("Floor number must be less than or equal to 500 ({PropertyName})");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Beds number must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(10).WithMessage("Beds number must be less than or equal to 10 ({PropertyName})");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("Area must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(1000).WithMessage("Area must be less than or equal to 1000 ({PropertyName})");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0 ({PropertyName})")
                .LessThanOrEqualTo(double.MaxValue).WithMessage($"Price must be less than or equal to {double.MaxValue} ({{PropertyName}})");

            RuleFor(x => x.HotelId)
                .NotNull().WithMessage("Hotel id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Hotel id must be not null ({PropertyName})");
        }
    }
}
