using FluentValidation;
using HotelReservation.API.Application.Commands.RoomView;

namespace HotelReservation.API.Application.Validation.RoomView
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
