using Contracts.DataTransferObjects.Validators;
using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class AddPaypalPayloadValidator : AbstractValidator<Payloads.AddPaypal>
{
    public AddPaypalPayloadValidator()
    {
        RuleFor(request => request.Amount)
            .SetValidator(new MoneyValidator())
            .OverridePropertyName(string.Empty);

        RuleFor(request => request.PayPal)
            .SetValidator(new PayPalValidator())
            .OverridePropertyName(string.Empty);
    }
}