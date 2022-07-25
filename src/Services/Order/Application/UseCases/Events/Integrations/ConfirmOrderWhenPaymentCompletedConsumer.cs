﻿using Application.EventStore;
using Contracts.Services.Payment;
using MassTransit;
using Command = Contracts.Services.Order.Command;

namespace Application.UseCases.Events.Integrations;

public class ConfirmOrderWhenPaymentCompletedConsumer : IConsumer<DomainEvent.PaymentCompleted>
{
    private readonly IOrderEventStoreService _eventStore;

    public ConfirmOrderWhenPaymentCompletedConsumer(IOrderEventStoreService eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task Consume(ConsumeContext<DomainEvent.PaymentCompleted> context)
    {
        var order = await _eventStore.LoadAsync(context.Message.OrderId, context.CancellationToken);
        order.Handle(new Command.ConfirmOrder(context.Message.OrderId));
        await _eventStore.AppendAsync(order, context.CancellationToken);
    }
}