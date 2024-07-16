using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Warehouse.Product;

public static class DomainEvent
{
    public record ProductRegistered(
        string ProductId,
        string ProductName,
        string Description,
        string Weight,
        string Length,
        string Width,
        string Height,
        string Brand,
        string Category,
        string Unit,
        string PictureUri,
        string Sku,
        string Currency,
        string Amount,
        ulong Version) : Message, IDomainEvent;
}