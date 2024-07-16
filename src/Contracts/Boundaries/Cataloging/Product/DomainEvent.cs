using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Cataloging.Product;

public static class DomainEvent
{
    public record ProductRegistered(string ProductId, string InventoryItemId,string ProductName, string CostCurrency, string CostAmount, string InitialInventory, ulong Version) 
        : Message, IDomainEvent;

    public record ProductInventoryTaken(string ProductId, string Quantity, string NewInventory, ulong Version)
        : Message, IDomainEvent;

    public record ProductInventoryReturned(string ProductId, string Quantity, string NewInventory, ulong Version)
        : Message, IDomainEvent;

    public record ProductInventoryUpdated(string ProductId, string NewInventory, ulong NewInventoryItemVersion, ulong Version)
        : Message, IDomainEvent;

    public record ProductDepleted(string ProductId, string NewInventory, ulong NewInventoryItemVersion, ulong Version)
        : Message, IDomainEvent;
    
    public record ProductInventoryDecreased(string ProductId, string NewInventory, ulong NewInventoryItemVersion, ulong Version)
        : Message, IDomainEvent;
    
    public record ProductInventoryIncreased(string ProductId, string NewInventory, ulong NewInventoryItemVersion, ulong Version)
        : Message, IDomainEvent;
}