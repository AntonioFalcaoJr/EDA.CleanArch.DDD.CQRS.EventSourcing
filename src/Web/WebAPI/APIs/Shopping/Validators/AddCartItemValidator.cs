using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class AddCartItemValidator : AbstractValidator<Commands.AddCartItem>
{
    public AddCartItemValidator()
    {
        RuleFor(request => request.CartId)
            .NotEmpty();

        RuleFor(request => request.Payload)
            .SetValidator(new AddCartItemPayloadValidator())
            .OverridePropertyName(string.Empty);
    }
}