using Contracts.Abstractions.Messages;
using Contracts.Boundaries.Cataloging.CatalogItem;
using Domain.Abstractions.Aggregates;
using Domain.Aggregates.Catalogs;
using Domain.Aggregates.Products;
using Domain.ValueObjects;
using Version = Domain.ValueObjects.Version;

namespace Domain.Aggregates.CatalogItems;

public class CatalogItem : AggregateRoot<CatalogItemId>
{
    public AppId AppId { get; private set; } = AppId.Undefined;
    public CatalogId CatalogId { get; private set; } = CatalogId.Undefined;
    public ProductId ProductId { get; private set; } = ProductId.Undefined;
    public Quantity Quantity { get; private set; } = Quantity.Zero;

    public static CatalogItem Create(AppId appId, CatalogId catalogId, ProductId productId, Quantity quantity)
    {
        CatalogItem item = new();
        DomainEvent.CatalogItemCreated @event = new(item.Id, appId, catalogId, productId, quantity, Version.Initial);
        item.RaiseEvent(@event);
        return item;
    }

    public void Remove() 
        => RaiseEvent(new DomainEvent.CatalogItemRemoved(CatalogId, Id, Version.Next));

    protected override void ApplyEvent(IDomainEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.CatalogItemCreated @event)
    {
        Id = (CatalogItemId)@event.ItemId;
        AppId = (AppId)@event.AppId;
        CatalogId = (CatalogId)@event.CatalogId;
        ProductId = (ProductId)@event.ProductId;
        Quantity = (Quantity)@event.Quantity;
    }

    private void When(DomainEvent.CatalogItemRemoved _)
        => IsDeleted = true;
}