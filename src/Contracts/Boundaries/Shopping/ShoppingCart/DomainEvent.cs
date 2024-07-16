using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Shopping.ShoppingCart;

public static class DomainEvent
{
    public record CartCheckedOut(string CartId, string Status, ulong Version) : Message, IDomainEvent;

    public record ShoppingStarted(string CartId, string CustomerId, string Status, ulong Version)
        : Message, IDomainEvent;

    public record CartDiscarded(string CartId, string Status, ulong Version) : Message, IDomainEvent;

    public record CartItemAdded(string CartId, string ItemId, string ProductId, string ProductName, string PictureUri,
        string Sku, string Quantity, IDictionary<string, string> Prices, IDictionary<string, string> Totals,
        ulong Version) : Message, IDomainEvent;

    public record CartItemDecreased(string CartId, string ItemId, string ProductId, string NewQuantity, 
        IDictionary<string, string> Prices, IDictionary<string, string> Totals, ulong Version) : Message, IDomainEvent;

    public record CartItemIncreased(string CartId, string ItemId, string ProductId, string NewQuantity, 
        IDictionary<string, string> Prices, IDictionary<string, string> Totals, ulong Version) : Message, IDomainEvent;

    public record CartItemRemoved(string CartId, string ItemId, string ProductId, IDictionary<string, string> Prices, 
        IDictionary<string, string> Totals, ulong Version) : Message, IDomainEvent;
}