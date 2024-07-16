using Application.Abstractions;
using Contracts.Boundaries.Cataloging.Catalog;

namespace Application.UseCases.Events;

public interface IProjectCatalogItemCardWhenCatalogChangedInteractor : IInteractor<DomainEvent.CatalogItemAdded>;

public class ProjectCatalogItemCardWhenCatalogChangedInteractor(IProjectionGateway<Projection.CatalogItemCard> projectionGateway)
    : IProjectCatalogItemCardWhenCatalogChangedInteractor
{
    public async Task InteractAsync(DomainEvent.CatalogItemAdded @event, CancellationToken cancellationToken)
    {
        Projection.CatalogItemCard card = new(
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