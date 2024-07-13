using Application.Services;
using Domain.Aggregates.CatalogItems;
using Domain.Aggregates.Products;
using MediatR;

namespace Application.UseCases.CatalogItems.Commands;

public record RemoveCatalogItem(CatalogItemId ItemId) : IRequest;

public class RemoveCatalogItemInteractor(IApplicationService service) : IRequestHandler<RemoveCatalogItem>
{
    public async Task Handle(RemoveCatalogItem cmd, CancellationToken cancellationToken)
    {
        var item = await service.LoadAggregateAsync<CatalogItem, CatalogItemId>(cmd.ItemId, cancellationToken);
        var product = await service.LoadAggregateAsync<Product, ProductId>(item.ProductId, cancellationToken);

        item.Remove();
        product.ReturnInventory(item.Quantity);

        // TODO: The design should be improved to do not handle multiple aggregates in a single transaction
        await service.AppendEventsAsync<CatalogItem, CatalogItemId>(item, cancellationToken);
        await service.AppendEventsAsync<Product, ProductId>(product, cancellationToken);
    }
}