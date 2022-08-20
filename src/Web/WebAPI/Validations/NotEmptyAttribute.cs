﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class NotEmptyAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "The {0} must not be empty/default";

    public NotEmptyAttribute()
        : base(DefaultErrorMessage) { }

    public override bool IsValid(object value)
        => value is Guid guid && guid != Guid.Empty;
}