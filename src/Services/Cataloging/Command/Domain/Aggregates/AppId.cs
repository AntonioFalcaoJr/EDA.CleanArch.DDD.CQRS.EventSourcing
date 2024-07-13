using Domain.Abstractions.Identities;

namespace Domain.Aggregates;

public record AppId : GuidIdentifier
{
    public AppId() { }
    public AppId(string value) : base(value) { }
    public AppId(Guid value) : base(value) { }

    public static AppId New => new();
    public static readonly AppId Undefined = new(Guid.Empty);

    public static explicit operator AppId(string value) => new(value);
    public override string ToString() => base.ToString();
}