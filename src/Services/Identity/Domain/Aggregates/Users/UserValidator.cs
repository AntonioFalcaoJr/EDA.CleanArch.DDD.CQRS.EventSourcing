﻿using System;
using Domain.Abstractions.Validators;
using FluentValidation;

namespace Domain.Aggregates.Users
{
    public class UserValidator : EntityValidator<User, Guid>
    {
        public UserValidator()
        {
            RuleFor(user => user.Password)
                .NotNull()
                .NotEmpty()
                .Equal(user => user.PasswordConfirmation);

            RuleFor(user => user.PasswordConfirmation)
                .NotNull()
                .NotEmpty()
                .Equal(user => user.Password);

            RuleFor(user => user.Login)
                .NotNull()
                .NotEmpty();
        }
    }
}