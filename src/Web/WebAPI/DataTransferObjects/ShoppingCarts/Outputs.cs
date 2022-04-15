﻿using ECommerce.Abstractions.Messages.Queries.Paging;

namespace WebAPI.DataTransferObjects.ShoppingCarts;

public static class Outputs
{
    public record ShoppingCart(Guid Id, Guid CustomerId, IEnumerable<ShoppingCartItem> Items, IEnumerable<string> PaymentMethods, decimal Total);

    public record ShoppingCartItem(Guid Id, Guid ProductId, string ProductName, decimal UnitPrice, int Quantity, string PictureUrl);

    public record ShoppingCartItems(IEnumerable<ShoppingCartItem> Items, PageInfo PageInfo) : IPagedResult<ShoppingCartItem>;
}