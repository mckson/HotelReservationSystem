using FluentValidation;
using HotelReservation.API.Application.Commands.Account;

namespace HotelReservation.API.Application.Validation.Account
{
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        public AuthenticateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Email must be nonempty ({PropertyName})")
                .EmailAddress().WithMessage("Invalid input value {PropertyValue}. It must be email ({PropertyName})");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Password must be nonempty ({PropertyName})");
        }
    }
}
