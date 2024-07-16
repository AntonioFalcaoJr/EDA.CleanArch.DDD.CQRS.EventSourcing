using Contracts.Abstractions.Messages;
using Contracts.Boundaries.Cataloging.Catalog;
using Infrastructure.EventBus.Consumers.Events;
using MassTransit;

namespace Infrastructure.EventBus.DependencyInjection.Extensions;

internal static class RabbitMqBusFactoryConfiguratorExtensions
{
    public static void ConfigureEventReceiveEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogGridItemWhenCatalogChangedConsumer, DomainEvent.CatalogCreated>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogGridItemWhenCatalogChangedConsumer, DomainEvent.CatalogDeleted>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogGridItemWhenCatalogChangedConsumer, DomainEvent.CatalogInactivated>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogGridItemWhenCatalogChangedConsumer, DomainEvent.CatalogActivated>(context);

        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemListItemWhenCatalogChangedConsumer, DomainEvent.CatalogDescriptionChanged>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemListItemWhenCatalogChangedConsumer, DomainEvent.CatalogTitleChanged>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemListItemWhenCatalogChangedConsumer, DomainEvent.CatalogDeleted>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemListItemWhenCatalogChangedConsumer, DomainEvent.CatalogItemAdded>(context);
        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemListItemWhenCatalogChangedConsumer, DomainEvent.CatalogItemRemoved>(context);

        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemCardWhenCatalogChangedConsumer, DomainEvent.CatalogItemAdded>(context);

        cfg.ConfigureEventReceiveEndpoint<ProjectCatalogItemDetailsWhenCatalogChangedConsumer, DomainEvent.CatalogItemAdded>(context);
    }

    private static void ConfigureEventReceiveEndpoint<TConsumer, TEvent>(this IRabbitMqBusFactoryConfigurator bus, IRegistrationContext context)
        where TConsumer : class, IConsumer
        where TEvent : class, IEvent
        => bus.ReceiveEndpoint(
            queueName: $"cataloging.query.{typeof(TConsumer).ToKebabCaseString()}.{typeof(TEvent).ToKebabCaseString()}",
            configureEndpoint: endpoint =>
            {
                endpoint.ConfigureConsumeTopology = false;
                endpoint.Bind<TEvent>();
                endpoint.ConfigureConsumer<TConsumer>(context);
            });
}