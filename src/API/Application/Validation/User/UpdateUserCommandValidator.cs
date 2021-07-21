using System;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.Core.Internal;
using FluentValidation;
using HotelReservation.API.Application.Commands.User;

namespace HotelReservation.API.Application.Validation.User
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Id must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Id must be not empty ({PropertyName})");

            RuleFor(x => x.Email)
               .NotNull().WithMessage("Email must be not null ({PropertyName})")
               .NotEmpty().WithMessage("Email must be not empty ({PropertyName})")
               .EmailAddress().WithMessage("Input value {PropertyValue} must be email ({PropertyName})");

            RuleFor(x => x.OldPassword)
                .NotNull().When(x => !x.NewPassword.IsNullOrEmpty()).WithMessage("Old password is required to create new one ({PropertyName})")
                .NotEmpty().When(x => !x.NewPassword.IsNullOrEmpty()).WithMessage("Old password is required to create new one ({PropertyName})");

            RuleFor(x => x.NewPassword);

            RuleFor(x => x.PasswordConfirm)
                .NotNull().When(x => !x.NewPassword.IsNullOrEmpty()).WithMessage("Password confirmation is required ({PropertyName})")
                .NotEmpty().When(x => !x.NewPassword.IsNullOrEmpty()).WithMessage("Password confirmation is required ({PropertyName})")
                .Equal(x => x.NewPassword).When(x => x.NewPassword != null).WithMessage("Password mismatch ({PropertyName})");

            RuleFor(x => x.PhoneNumber)
                .Must(number => number != null && Regex.IsMatch(
                    number,
                    @"^\+?\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"))
                .When(x => x.PhoneNumber != null)
                .WithMessage("Input value {PropertyValue} must be phone number ({PropertyName})");

            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => dateOfBirth < DateTime.Now).When(x => x.DateOfBirth.HasValue)
                .WithMessage("Birth date can not be after current date ({PropertyName})");

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("Surname must be specified ({PropertyName})")
                .NotEmpty().WithMessage("Surname must be specified ({PropertyName})");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("Name must be specified ({PropertyName})")
                .NotEmpty().WithMessage("Name must be specified ({PropertyName})");

            // RuleFor(x => x.Roles)
            //     .NotNull().WithMessage("One or more role is required ({PropertyName})")
            //     .Must(roles => roles != null && roles.Any()).WithMessage("One or more role is required ({PropertyName})");
        }
    }
}
