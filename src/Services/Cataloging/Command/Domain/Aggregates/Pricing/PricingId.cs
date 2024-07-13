using Domain.Abstractions.Identities;

namespace Domain.Aggregates.Pricing;

public record PricingId : GuidIdentifier
{
    public PricingId() { }
    public PricingId(string value) : base(value) { }
    public PricingId(Guid value) : base(value) { }

    public static PricingId New => new();
    public static readonly PricingId Undefined = new(Guid.Empty);

    public static explicit operator PricingId(string value) => new(value);
    public override string ToString() => base.ToString();
}