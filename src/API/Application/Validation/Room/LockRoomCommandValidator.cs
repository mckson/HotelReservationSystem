using FluentValidation;
using HotelReservation.API.Application.Commands.Room;

namespace HotelReservation.API.Application.Validation.Room
{
    public class LockRoomCommandValidator : AbstractValidator<LockRoomCommand>
    {
        public LockRoomCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Locking room id is required {PropertyName}");
        }
    }
}
