using Application.Services;
using Domain.Aggregates;
using Domain.Aggregates.CatalogItems;
using Domain.Aggregates.Catalogs;
using Domain.Aggregates.Products;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.CatalogItems.Commands;

public record CreateCatalogItem(AppId AppId, CatalogId CatalogId, ProductId ProductId, Quantity Quantity) : IRequest;

public class CreateCatalogItemInteractor(IApplicationService service) : IRequestHandler<CreateCatalogItem>
{
    public async Task Handle(CreateCatalogItem cmd, CancellationToken token)
    {
        var product = await service.LoadAggregateAsync<Product, ProductId>(cmd.ProductId, token);

        product.TakeInventory(cmd.Quantity);

        var newItem = CatalogItem.Create(
            cmd.AppId,
            cmd.CatalogId,
            cmd.ProductId,
            cmd.Quantity);

        // TODO: The design should be improved to do not handle multiple aggregates in a single transaction
        await service.AppendEventsAsync<CatalogItem, CatalogItemId>(newItem, token);
        await service.AppendEventsAsync<Product, ProductId>(product, token);
    }
}