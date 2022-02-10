﻿using System;
using ECommerce.Abstractions.Messages.Queries;

namespace ECommerce.Contracts.ShoppingCart;

public static class Queries
{
    public record GetShoppingCart(Guid CartId) : Query(CorrelationId: CartId);

    public record GetCustomerShoppingCart(Guid CustomerId) : Query(CorrelationId: CustomerId);

    public record GetShoppingCartItems(Guid CartId) : Query(CorrelationId: CartId);
}