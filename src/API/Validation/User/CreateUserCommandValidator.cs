﻿using FluentValidation;
using HotelReservation.API.Commands.User;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HotelReservation.API.Validation.User
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("Email must be not null ({PropertyName})")
                .NotEmpty().WithMessage("Email must be not empty ({PropertyName})")
                .EmailAddress().WithMessage("Input value {PropertyValue} must be email ({PropertyName})");

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required ({PropertyName})")
                .NotEmpty().WithMessage("Password is required ({PropertyName})");

            RuleFor(x => x.PasswordConfirm)
                .NotNull().WithMessage("Password confirmation is required ({PropertyName})")
                .NotEmpty().WithMessage("Password confirmation is required ({PropertyName})")
                .Equal(x => x.Password).WithMessage("Password mismatch ({PropertyName})");

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("Phone is required ({PropertyName})")
                .NotEmpty().WithMessage("Phone is required ({PropertyName})")
                .Must(number => number != null && Regex.IsMatch(number, @"^\+?\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"))
                .WithMessage("Input value {PropertyValue} must be phone number ({PropertyName})");

            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Date of Birth is required ({PropertyName})")
                .NotEmpty().WithMessage("Date of Birth is required ({PropertyName})")
                .Must(dateOfBirth => dateOfBirth < DateTime.Now)
                .WithMessage("Birth date can not be after current date ({PropertyName})");

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("Surname must be specified ({PropertyName})")
                .NotEmpty().WithMessage("Surname must be specified ({PropertyName})");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("Name must be specified ({PropertyName})")
                .NotEmpty().WithMessage("Name must be specified ({PropertyName})");

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("One or more role is required ({PropertyName})")
                .Must(roles => roles != null && roles.Any()).WithMessage("One or more role is required ({PropertyName})");
        }
    }
}
