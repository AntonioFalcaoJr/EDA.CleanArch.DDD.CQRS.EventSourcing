using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class StartShoppingPayloadValidator : AbstractValidator<Payloads.StartShopping>
{
    public StartShoppingPayloadValidator() 
        => RuleFor(request => request.CustomerId).NotEmpty();
}