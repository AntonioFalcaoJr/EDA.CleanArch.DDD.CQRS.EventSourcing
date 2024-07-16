using Contracts.Abstractions.Messages;

namespace Contracts.Boundaries.Shopping.ShoppingCart;

public static class Command
{
    public record AddCartItem(string CartId, string ProductId, ushort Quantity, string Currency) : Message, ICommand;

    public record ChangeCartItemQuantity(string CartId, string ProductId, ushort NewQuantity) : Message, ICommand;

    public record CheckOutCart(string CartId) : Message, ICommand;

    public record DiscardCart(string CartId) : Message, ICommand;

    public record RebuildCartProjection(string Projection) : Message, ICommand;

    

    public record StartShopping(string CustomerId) : Message, ICommand;
}