using Application.Services;
using Contracts.Boundaries.Warehouse.Inventory;
using Domain.Aggregates;
using Domain.Aggregates.Products;
using Domain.ValueObjects;
using MediatR;

namespace Application.UseCases.Products.Events;

public class RegisterProductInteractor(IApplicationService service) : IRequestHandler<DomainEvent.InventoryItemReceived>
{
    public Task Handle(DomainEvent.InventoryItemReceived @event, CancellationToken cancellationToken)
    {
        var product = Product.Register(
            (InventoryItemId)@event.InventoryItemId,
            (ProductName)@event.Name,
            new((Amount)@event.Cost, (Currency)@event.Currency),
            (Quantity)@event.Quantity
        );

        return service.AppendEventsAsync<Product, ProductId>(product, cancellationToken);
    }
}