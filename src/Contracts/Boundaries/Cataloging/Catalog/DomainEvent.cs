using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Cataloging.Catalog;

public static class DomainEvent
{
    public record CatalogCreated(string CatalogId, string AppId, string Title, string Description, ulong Version) : Message, IDomainEvent;

    public record CatalogDeleted(string CatalogId, string Status, ulong Version) : Message, IDomainEvent;

    public record CatalogActivated(string CatalogId, string Status, ulong Version) : Message, IDomainEvent;

    public record CatalogInactivated(string CatalogId, string Status, ulong Version) : Message, IDomainEvent;

    public record CatalogTitleChanged(string CatalogId, string Title, ulong Version) : Message, IDomainEvent;

    public record CatalogDescriptionChanged(string CatalogId, string Description, ulong Version) : Message, IDomainEvent;

    public record CatalogItemAdded(string CatalogId, string ItemId, string InventoryId, Dto.Product Product, Dto.Money UnitPrice, string Sku, int Quantity, ulong Version) : Message, IDomainEvent;

    public record CatalogItemRemoved(string CatalogId, string ItemId, ulong Version) : Message, IDomainEvent;

    public record CatalogItemIncreased(string CatalogId, string ItemId, string InventoryId, int Quantity, ulong Version) : Message, IDomainEvent;
}