using Application.Abstractions;
using Contracts.Boundaries.Cataloging.Catalog;

namespace Application.UseCases.Events;

public interface IProjectCatalogItemDetailsWhenCatalogChangedInteractor : IInteractor<DomainEvent.CatalogItemAdded>;

public class ProjectCatalogItemDetailsWhenCatalogChangedInteractor(IProjectionGateway<Projection.CatalogItemDetails> projectionGateway)
    : IProjectCatalogItemDetailsWhenCatalogChangedInteractor
{
    public async Task InteractAsync(DomainEvent.CatalogItemAdded @event, CancellationToken cancellationToken)
    {
        Projection.CatalogItemDetails card = new(
            @event.ItemId,
            @event.CatalogId,
            @event.Product,
            @event.UnitPrice,
            "image url",
            false,
            @event.Version);

        await projectionGateway.ReplaceInsertAsync(card, cancellationToken);
    }
}