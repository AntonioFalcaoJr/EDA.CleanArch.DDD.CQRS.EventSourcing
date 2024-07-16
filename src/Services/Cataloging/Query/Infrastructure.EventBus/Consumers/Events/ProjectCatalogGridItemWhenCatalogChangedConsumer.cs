using Application.UseCases.Events;
using Contracts.Boundaries.Cataloging.Catalog;
using MassTransit;

namespace Infrastructure.EventBus.Consumers.Events;

public class ProjectCatalogGridItemWhenCatalogChangedConsumer(IProjectCatalogGridItemWhenCatalogChangedInteractor interactor)
    :
        IConsumer<DomainEvent.CatalogActivated>,
        IConsumer<DomainEvent.CatalogCreated>,
        IConsumer<DomainEvent.CatalogInactivated>,
        IConsumer<DomainEvent.CatalogDeleted>,
        IConsumer<DomainEvent.CatalogDescriptionChanged>,
        IConsumer<DomainEvent.CatalogTitleChanged>
{
    public Task Consume(ConsumeContext<DomainEvent.CatalogActivated> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogCreated> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogInactivated> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogDeleted> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogDescriptionChanged> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);

    public Task Consume(ConsumeContext<DomainEvent.CatalogTitleChanged> context)
        => interactor.InteractAsync(context.Message, context.CancellationToken);
}