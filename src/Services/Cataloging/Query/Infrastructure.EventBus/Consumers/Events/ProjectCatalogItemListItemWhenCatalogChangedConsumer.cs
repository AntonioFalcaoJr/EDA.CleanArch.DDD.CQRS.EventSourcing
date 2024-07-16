using Application.UseCases.Events;
using Contracts.Boundaries.Cataloging.Catalog;
using MassTransit;

namespace Infrastructure.EventBus.Consumers.Events;

public class ProjectCatalogItemListItemWhenCatalogChangedConsumer(IProjectCatalogItemListItemWhenCatalogChangedInteractor interactor)
    :
        IConsumer<DomainEvent.CatalogDeleted>,
        IConsumer<DomainEvent.CatalogItemAdded>,
        IConsumer<DomainEvent.CatalogItemRemoved>
{
    public Task Consume(ConsumeContext<DomainEvent.CatalogDeleted> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogItemAdded> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogItemRemoved> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);
}