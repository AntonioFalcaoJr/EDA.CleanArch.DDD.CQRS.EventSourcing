using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class AddCartItemPayloadValidator : AbstractValidator<Payloads.AddCartItem>
{
    public AddCartItemPayloadValidator()
    {
        RuleFor(request => request.ProductId).NotEmpty();
        RuleFor(request => request.Quantity).GreaterThan(0);
    }
}