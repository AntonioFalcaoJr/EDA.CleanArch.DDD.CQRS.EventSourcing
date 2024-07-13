using Domain.Abstractions.Identities;

namespace Domain.Aggregates.Products;

public record ProductId : GuidIdentifier
{
    public ProductId() { }
    public ProductId(string value) : base(value) { }
    public ProductId(Guid value) : base(value) { }

    public static ProductId New => new();
    public static readonly ProductId Undefined = new(Guid.Empty);

    public static explicit operator ProductId(string value) => new(value);
    public override string ToString() => base.ToString();
}