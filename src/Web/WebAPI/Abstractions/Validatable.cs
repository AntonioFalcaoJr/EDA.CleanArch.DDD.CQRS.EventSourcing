using FluentValidation;
using Grpc.Core;

namespace WebAPI.Abstractions;

public abstract record TheCommand<TClient, TValidator>(TClient Client, CancellationToken Token)
    : Validatable<TValidator>, INewCommand<TClient>
    where TValidator : IValidator, new()
    where TClient : ClientBase;

public abstract record Validatable<TValidator>
    where TValidator : IValidator, new()
{
    private readonly TValidator _validator = new();

    public bool IsValid(out IDictionary<string, string[]> errors)
    {
        var result = _validator.Validate(new ValidationContext<Validatable<TValidator>>(this));
        errors = result.ToDictionary();
        return result.IsValid;
    }
}

//////////////////////////

public interface IVeryNewCommand<out TClient, TValidator> : INewCommand<TClient>, IValidatable<TValidator>
    where TClient : ClientBase 
    where TValidator : IValidator, new();

public interface IValidatable<TValidator> : INewRequest
    where TValidator : IValidator, new()
{
    new bool IsValid(out IDictionary<string, string[]> errors)
    {
        var context = new ValidationContext<IValidatable<TValidator>>(this);
        var result = new TValidator().Validate(context);
        errors = result.ToDictionary();
        return result.IsValid;
    }
}