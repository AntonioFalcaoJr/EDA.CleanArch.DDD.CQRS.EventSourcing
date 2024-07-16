using Application.UseCases.Events;
using Contracts.Boundaries.Cataloging.Catalog;
using MassTransit;

namespace Infrastructure.EventBus.Consumers.Events;

public class ProjectCatalogItemDetailsWhenCatalogChangedConsumer(IProjectCatalogItemDetailsWhenCatalogChangedInteractor interactor)
    : IConsumer<DomainEvent.CatalogItemAdded>
{
    public Task Consume(ConsumeContext<DomainEvent.CatalogItemAdded> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);
}