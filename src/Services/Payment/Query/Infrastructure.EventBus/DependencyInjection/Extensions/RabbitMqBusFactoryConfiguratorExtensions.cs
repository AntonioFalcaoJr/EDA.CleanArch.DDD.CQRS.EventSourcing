﻿using Contracts.Abstractions.Messages;
using Contracts.Services.Payment;
using Infrastructure.EventBus.Consumers.Events;
using MassTransit;

namespace Infrastructure.EventBus.DependencyInjection.Extensions;

internal static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentRequested>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodAuthorized>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodDenied>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodCanceled>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodCancellationDenied>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodRefunded>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentMethodWhenChangedConsumer, DomainEvent.PaymentMethodRefundDenied>(context);
        
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentWhenChangedConsumer, DomainEvent.PaymentRequested>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectPaymentWhenChangedConsumer, DomainEvent.PaymentCanceled>(context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
        where TConsumer : class, IConsumer
        where TEvent : class, IEvent
        => bus.ReceiveEndpoint(
            queueName: $"payment.query-stack.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
            configureEndpoint: endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind<TEvent>();
                endpoint.ConfigureConsumer<TConsumer>(context);
            });
}