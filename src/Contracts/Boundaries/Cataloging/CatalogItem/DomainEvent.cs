using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Cataloging.CatalogItem;

public static class DomainEvent
{
    public record CatalogItemCreated(string CatalogId, string AppId, string ItemId, string ProductId, string Quantity, ulong Version) : Message, IDomainEvent;

    public record CatalogItemRemoved(string CatalogId, string CatalogItemId, ulong Version) : Message, IDomainEvent;

    public record CatalogItemIncreased(Guid CatalogId, Guid ItemId, Guid InventoryId, int Quantity, ulong Version) : Message, IDomainEvent;
}