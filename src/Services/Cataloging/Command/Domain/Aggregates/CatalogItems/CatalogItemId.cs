using Domain.Abstractions.Identities;

namespace Domain.Aggregates.CatalogItems;

public record CatalogItemId : GuidIdentifier
{
    public CatalogItemId() { }
    public CatalogItemId(string value) : base(value) { }
    public CatalogItemId(Guid value) : base(value) { }

    public static CatalogItemId New => new();
    public static readonly CatalogItemId Undefined = new(Guid.Empty);

    public static explicit operator CatalogItemId(string value) => new(value);
    public override string ToString() => base.ToString();
}