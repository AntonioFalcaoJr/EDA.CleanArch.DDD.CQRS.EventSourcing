﻿using Application.UseCases.Events.Integrations;
using Application.UseCases.Events.Projections;
using ECommerce.Abstractions.Messages.Events;
using ECommerce.Contracts.ShoppingCart;
using MassTransit;

namespace Infrastructure.DependencyInjection.Extensions;

internal static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext registration)
    {
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartCreated>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartItemAdded>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartItemRemoved>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CreditCardAdded>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.PayPalAdded>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.BillingAddressChanged>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.ShippingAddressAdded>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartCheckedOut>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartItemIncreased>(registration);
        cfg.ConfigureEventReceiveEndpoint<ProjectCartDetailsWhenCartChangedConsumer, DomainEvents.CartItemDecreased>(registration);
        cfg.ConfigureEventReceiveEndpoint<PublishCartSubmittedWhenCartCheckedOutConsumer, DomainEvents.CartCheckedOut>(registration);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext registration)
        where TConsumer : class, IConsumer
        where TEvent : class, IEvent
        => bus.ReceiveEndpoint(
            queueName: $"shopping-cart.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
            configureEndpoint: endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind<TEvent>();
                endpoint.ConfigureConsumer<TConsumer>(registration);
            });
}