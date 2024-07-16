using Application.Abstractions;
using Application.Projections;
using Contracts.Boundaries.Cataloging.Product;

namespace Application.UseCases.Events;

public class ProjectSearchableProductInteractor(IProjectionGateway<SearchableProduct> gateway)
    : IInteractor<DomainEvent.ProductRegistered>
{
    public Task InteractAsync(DomainEvent.ProductRegistered @event, CancellationToken cancellationToken)
    {
        SearchableProduct searchableProduct = new(
            @event.ProductId,
            @event.ProductName,
            "@event.Description",
            false,
            @event.Version);

        return gateway.IndexAsync(searchableProduct, cancellationToken);
    }
}