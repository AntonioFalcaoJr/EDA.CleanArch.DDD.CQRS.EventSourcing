using Contracts.Abstractions.Messages;
using Contracts.Boundaries.Cataloging.Product;
using Domain.Abstractions.Aggregates;
using Domain.ValueObjects;
using static Domain.Exceptions;
using Version = Domain.ValueObjects.Version;

namespace Domain.Aggregates.Products;

public class Product : AggregateRoot<ProductId>
{
    public InventoryItemId InventoryItemId { get; private set; } = InventoryItemId.Undefined;
    public Version InventoryItemVersion { get; private set; } = Version.Zero;
    public ProductName Name { get; private set; } = ProductName.Undefined;
    public PictureUri PictureUri { get; private set; } = PictureUri.Undefined;
    public Sku Sku { get; private set; } = Sku.Undefined;
    public Quantity Inventory { get; private set; } = Quantity.Zero;
    public Money Cost { get; private set; } = Money.Zero(Currency.Undefined);

    public static Product Register(InventoryItemId inventoryItemId, ProductName name, Cost cost, Quantity quantity)
    {
        Product product = new();
        DomainEvent.ProductRegistered @event = new(ProductId.New, inventoryItemId, name, cost.Currency, cost.Amount, quantity, Version.Initial);
        product.RaiseEvent(@event);
        return product;
    }

    public void TakeInventory(Quantity quantity)
    {
        ProductInventoryNotEnough.ThrowIf(quantity > Inventory);
        
        var newInventory = Inventory - quantity;
        RaiseEvent(new DomainEvent.ProductInventoryTaken(Id, quantity, newInventory, Version.Next));
    }
    
    public void ReturnInventory(Quantity quantity)
    {
        var newInventory = Inventory + quantity;
        RaiseEvent(new DomainEvent.ProductInventoryReturned(Id, quantity, newInventory, Version.Next));
    }
    
    public void UpdateInventory(Quantity newQuantity, Version newInventoryItemVersion)
    {
        if (InventoryItemVersion >= newInventoryItemVersion) return;
        
        RaiseEvent(newQuantity switch
        {
            _ when newQuantity == Quantity.Zero => ProductDepleted(),
            _ when newQuantity < Inventory => ProductInventoryDecreased(),
            _ when newQuantity > Inventory => ProductInventoryIncreased(),
            _ => InvalidQuantity.Throw()
        });

        DomainEvent.ProductDepleted ProductDepleted()
            => new(Id, newQuantity, newInventoryItemVersion, Version.Next);

        DomainEvent.ProductInventoryDecreased ProductInventoryDecreased()
            => new(Id, newQuantity, newInventoryItemVersion, Version.Next);

        DomainEvent.ProductInventoryIncreased ProductInventoryIncreased()
            => new(Id, newQuantity, newInventoryItemVersion, Version.Next);
    }
    
    protected override void ApplyEvent(IDomainEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.ProductRegistered @event)
    {
        Id = (ProductId)@event.ProductId;
        Name = (ProductName)@event.ProductName;
        Cost = new((Amount)@event.CostAmount, (Currency)@event.CostCurrency);
        Inventory = (Quantity)@event.InitialInventory;
    }

    private void When(DomainEvent.ProductInventoryTaken @event)
        => Inventory = (Quantity)@event.NewInventory;
    
    private void When(DomainEvent.ProductInventoryReturned @event)
        => Inventory = (Quantity)@event.NewInventory;
    
    private void When(DomainEvent.ProductInventoryUpdated @event)
    {
        Inventory = (Quantity)@event.NewInventory;
        InventoryItemVersion = (Version)@event.NewInventoryItemVersion;
    }
}