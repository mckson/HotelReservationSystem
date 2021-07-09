using FluentValidation;
using HotelReservation.API.Commands.RoomView;

namespace HotelReservation.API.Validation.RoomView
{
    public class UpdateRoomViewValidator : AbstractValidator<UpdateRoomViewCommand>
    {
        public UpdateRoomViewValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Id must be not empty ({PropertyName})");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Name must be specified {PropertyName}")
                .NotEmpty().WithMessage("Name must be specified {PropertyName}");
        }
    }
}
