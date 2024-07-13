using Domain.Abstractions.Identities;

namespace Domain.Aggregates;

public record InventoryItemId : GuidIdentifier
{
    public InventoryItemId() { }
    public InventoryItemId(string value) : base(value) { }
    public InventoryItemId(Guid value) : base(value) { }

    public static InventoryItemId New => new();
    public static readonly InventoryItemId Undefined = new(Guid.Empty);

    public static explicit operator InventoryItemId(string value) => new(value);
    public override string ToString() => base.ToString();
}