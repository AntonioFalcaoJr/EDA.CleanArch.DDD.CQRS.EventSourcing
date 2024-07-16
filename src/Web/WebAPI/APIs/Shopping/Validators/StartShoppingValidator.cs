using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class StartShoppingValidator : AbstractValidator<Commands.StartShopping>
{
    public StartShoppingValidator()
        => RuleFor(request => request.Payload)
            .SetValidator(new StartShoppingPayloadValidator())
            .OverridePropertyName(string.Empty);
}