using FluentValidation;
using HotelReservation.API.Commands.RoomView;

namespace HotelReservation.API.Validation.RoomView
{
    public class CreateRoomVIewValidator : AbstractValidator<CreateRoomViewCommand>
    {
        public CreateRoomVIewValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be specified {PropertyName}")
                .NotEmpty().WithMessage("Name must be specified {PropertyName}");
        }
    }
}
