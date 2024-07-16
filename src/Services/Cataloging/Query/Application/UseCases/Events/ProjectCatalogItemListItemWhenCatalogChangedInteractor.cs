using Application.Abstractions;
using Contracts.Boundaries.Cataloging.Catalog;

namespace Application.UseCases.Events;

public interface IProjectCatalogItemListItemWhenCatalogChangedInteractor :
    IInteractor<DomainEvent.CatalogDeleted>,
    IInteractor<DomainEvent.CatalogItemAdded>,
    IInteractor<DomainEvent.CatalogItemRemoved>;

public class ProjectCatalogItemListItemWhenCatalogChangedInteractor(IProjectionGateway<Projection.CatalogItemListItem> projectionGateway)
    : IProjectCatalogItemListItemWhenCatalogChangedInteractor
{
    public async Task InteractAsync(DomainEvent.CatalogItemAdded @event, CancellationToken cancellationToken)
    {
        Projection.CatalogItemListItem listItem = new(
            @event.ItemId,
            @event.CatalogId,
            "@event.ProductId",
            @event.Product,
            false,
            @event.Version);

        await projectionGateway.ReplaceInsertAsync(listItem, cancellationToken);
    }

    public Task InteractAsync(DomainEvent.CatalogDeleted @event, CancellationToken cancellationToken)
        => projectionGateway.DeleteAsync(item => item.CatalogId == @event.CatalogId, cancellationToken);

    public Task InteractAsync(DomainEvent.CatalogItemRemoved @event, CancellationToken cancellationToken)
        => projectionGateway.DeleteAsync(@event.ItemId, cancellationToken);
}