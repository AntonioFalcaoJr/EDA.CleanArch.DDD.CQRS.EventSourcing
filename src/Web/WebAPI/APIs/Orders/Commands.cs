﻿using Contracts.Boundaries.Order;
using MassTransit;
using WebAPI.Abstractions;
using WebAPI.APIs.Orders.Validators;

namespace WebAPI.APIs.Orders;

public static class Commands
{
    public record CancelOrder(IBus Bus, string OrderId, CancellationToken CancellationToken)
        : Validatable<CancelOrderValidator>, ICommand<Command.CancelOrder>
    {
        public Command.CancelOrder Command => new(OrderId);
    }
}