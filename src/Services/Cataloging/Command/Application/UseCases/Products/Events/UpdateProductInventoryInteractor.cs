using Application.Services;
using Contracts.Boundaries.Warehouse.Inventory;
using Domain.Aggregates;
using Domain.Aggregates.Products;
using Domain.ValueObjects;
using MediatR;
using Version = Domain.ValueObjects.Version;

namespace Application.UseCases.Products.Events;

public class UpdateProductInventoryInteractor(IApplicationService service) :
    IRequestHandler<DomainEvent.InventoryItemCheckedIn>,
    IRequestHandler<DomainEvent.InventoryItemCheckedOut>
{
    public Task Handle(DomainEvent.InventoryItemCheckedIn @event, CancellationToken token)
        => UpdateInventoryAsync((InventoryItemId)@event.InventoryItemId, (Quantity)@event.NewQuantity, (Version)@event.Version, token);

    public Task Handle(DomainEvent.InventoryItemCheckedOut @event, CancellationToken token)
        => UpdateInventoryAsync((InventoryItemId)@event.InventoryItemId, (Quantity)@event.NewQuantity, (Version)@event.Version, token);

    private async Task UpdateInventoryAsync(InventoryItemId inventoryItemId, Quantity newQuantity, Version newVersion, CancellationToken token)
    {
        var product = await service.LoadAggregateAsync<Product, ProductId>(product => product.InventoryItemId == inventoryItemId, token);
        product.UpdateInventory(newQuantity, newVersion);
        await service.AppendEventsAsync<Product, ProductId>(product, token);
    }
}