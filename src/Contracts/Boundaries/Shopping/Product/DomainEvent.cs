using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Shopping.Product;

public static class DomainEvent
{
    public record ProductRegistered(string ProductId, string CatalogItemId, string Name, IDictionary<string, string> Prices, string Inventory, ulong Version) 
        : Message, IDomainEvent;

    public record ProductInventoryTaken(string ProductId, string Quantity, string NewInventory, ulong Version) 
        : Message, IDomainEvent;

    public record ProductInventoryReturned(string ProductId, string Quantity, string NewInventory, ulong Version) 
        : Message, IDomainEvent;
}