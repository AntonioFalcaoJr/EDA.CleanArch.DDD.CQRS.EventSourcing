using Contracts.DataTransferObjects.Validators;
using FluentValidation;

namespace WebAPI.APIs.Shopping.Validators;

public class AddDebitCardPayloadValidator : AbstractValidator<Payloads.AddDebitCard>
{
    public AddDebitCardPayloadValidator()
    {
        RuleFor(request => request.Amount)
            .SetValidator(new MoneyValidator())
            .OverridePropertyName(string.Empty);

        RuleFor(request => request.DebitCard)
            .SetValidator(new DebitCardValidator())
            .OverridePropertyName(string.Empty);
    }
}