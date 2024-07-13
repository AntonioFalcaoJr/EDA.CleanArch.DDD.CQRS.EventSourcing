using static Domain.Exceptions;

namespace Domain.Abstractions.Identities;

public interface IIdentifier;

public abstract record GuidIdentifier : IIdentifier
{
    private readonly Guid _value;

    protected GuidIdentifier(Guid value) => Value = value;
    protected GuidIdentifier() => Value = Guid.NewGuid();
    protected GuidIdentifier(string value) => InvalidIdentifier.ThrowIf(!Guid.TryParse(value, out _value));

    private Guid Value
    {
        get => _value;
        init => _value = value;
    }

    public static T New<T>(Guid value) where T : GuidIdentifier, new() => new() { Value = value };

    public static implicit operator string(GuidIdentifier id) => id.Value.ToString();
    public static implicit operator Guid(GuidIdentifier id) => id.Value;

    public static bool operator ==(GuidIdentifier id, string value) => id.Value.CompareTo(value) is 0;
    public static bool operator !=(GuidIdentifier id, string value) => id.Value.CompareTo(value) is not 0;

    public override string ToString() => Value.ToString();
}