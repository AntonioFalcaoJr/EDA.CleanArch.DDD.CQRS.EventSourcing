using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Warehouse.Inventory;

public static class DomainEvent
{
    public record InventoryItemReceived(string InventoryId, string InventoryItemId, string ProductId, string Name, string Brand, string Category, string Unit, string Currency, string Cost, int Quantity, string Sku, ulong Version) : Message, IDomainEvent;
    
    public record InventoryItemCheckedIn(string InventoryId, string InventoryItemId, string NewQuantity, ulong Version) : Message, IDomainEvent;
    
    public record InventoryItemCheckedOut(string InventoryId, string InventoryItemId, string NewQuantity, ulong Version) : Message, IDomainEvent;
    
    public record InventoryCreated(Guid InventoryId, Guid OwnerId, ulong Version) : Message, IDomainEvent;


    public record InventoryAdjustmentIncreased(Guid InventoryId, Guid ItemId, string Reason, int Quantity, ulong Version) : Message, IDomainEvent;

    public record InventoryAdjustmentDecreased(Guid InventoryId, Guid ItemId, string Reason, int Quantity, ulong Version) : Message, IDomainEvent;

    public record InventoryAdjustmentNotDecreased(Guid InventoryId, Guid ItemId, string Reason, int QuantityDesired, int QuantityAvailable, ulong Version) : Message, IDomainEvent;

    public record InventoryReserved(Guid InventoryId, Guid ItemId, Guid CatalogId, Guid CartId, Dto.Product Product, int Quantity, DateTimeOffset Expiration, ulong Version) : Message, IDomainEvent;

    public record StockDepleted(Guid InventoryId, Guid ItemId, Dto.Product Product, ulong Version) : Message, IDomainEvent;

    public record InventoryNotReserved(Guid InventoryId, Guid ItemId, Guid CartId, int QuantityDesired, int QuantityAvailable, ulong Version) : Message, IDomainEvent;

    public record InventoryItemIncreased(Guid InventoryId, Guid ItemId, int Quantity, ulong Version) : Message, IDomainEvent;

    public record InventoryItemDecreased(Guid InventoryId, Guid ItemId, int Quantity, ulong Version) : Message, IDomainEvent;
}