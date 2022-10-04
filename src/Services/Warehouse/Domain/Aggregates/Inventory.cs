﻿using Domain.Abstractions.Aggregates;
using Contracts.Abstractions.Messages;
using Contracts.Services.Warehouse;
using Domain.Entities.Adjustments;
using Domain.Entities.InventoryItems;
using Domain.ValueObjects.Products;

namespace Domain.Aggregates;

public class Inventory : AggregateRoot<Guid, InventoryValidator>
{
    private readonly List<InventoryItem> _items = new();
    private static DateTimeOffset Expiration => DateTimeOffset.Now.AddMinutes(5);

    public Guid OwnerId { get; private set; }

    public int TotalItems
        => Items.Count();

    public int TotalUnits
        => Items.Sum(item => item.Quantity);

    public IEnumerable<InventoryItem> Items
        => _items;

    public void Handle(Command.CreateInventory cmd)
        => RaiseEvent(new DomainEvent.InventoryCreated(Guid.NewGuid(), cmd.OwnerId));

    public void Handle(Command.ReceiveInventoryItem cmd)
        => RaiseEvent(_items
            .Where(inventoryItem => inventoryItem.Product == cmd.Product)
            .SingleOrDefault(inventoryItem => inventoryItem.Cost == cmd.Cost) is { IsDeleted: false } item
            ? new DomainEvent.InventoryItemIncreased(cmd.Id, item.Id, cmd.Quantity)
            : new DomainEvent.InventoryItemReceived(cmd.Id, Guid.NewGuid(), cmd.Product, cmd.Cost, cmd.Quantity, FormatSku(cmd.Product)));

    public void Handle(Command.DecreaseInventoryAdjust cmd)
    {
        if (_items.SingleOrDefault(inventoryItem => inventoryItem.Id == cmd.ItemId) is { IsDeleted: false } item)
            RaiseEvent(item.QuantityAvailable >= cmd.Quantity
                ? new DomainEvent.InventoryAdjustmentDecreased(cmd.Id, cmd.ItemId, cmd.Reason, cmd.Quantity)
                : new DomainEvent.InventoryAdjustmentNotDecreased(cmd.Id, cmd.ItemId, cmd.Reason, cmd.Quantity, item.QuantityAvailable));
    }

    public void Handle(Command.IncreaseInventoryAdjust cmd)
    {
        if (_items.SingleOrDefault(item => item.Id == cmd.ItemId) is { IsDeleted: false })
            RaiseEvent(new DomainEvent.InventoryAdjustmentIncreased(cmd.Id, cmd.ItemId, cmd.Reason, cmd.Quantity));
    }

    public void Handle(Command.ReserveInventoryItem cmd)
    {
        if (_items.SingleOrDefault(inventoryItem => inventoryItem.Sku == cmd.Sku) is { IsDeleted: false } item)
            RaiseEvent(item.QuantityAvailable switch
            {
                < 1 => new DomainEvent.StockDepleted(cmd.Id, item.Id, item.Product),
                var availability when availability >= cmd.Quantity => new DomainEvent.InventoryReserved(cmd.Id, cmd.CatalogId, cmd.CartId, item.Id, cmd.Sku, cmd.Quantity, Expiration),
                _ => new DomainEvent.InventoryNotReserved(Id, cmd.CartId, item.Id, cmd.Sku, cmd.Quantity, item.QuantityAvailable),
            });
    }

    protected override void ApplyEvent(IEvent @event)
        => When(@event as dynamic);

    private void When(DomainEvent.InventoryCreated @event)
        => (Id, OwnerId) = @event;

    private void When(DomainEvent.InventoryItemReceived @event)
        => _items.Add(new(@event.InventoryItemId, @event.Cost, @event.Product, @event.Quantity, @event.Sku));

    private void When(DomainEvent.InventoryItemIncreased @event)
        => _items
            .Single(item => item.Id == @event.InventoryItemId)
            .Increase(@event.Quantity);

    private void When(DomainEvent.InventoryItemDecreased @event)
        => _items
            .Single(item => item.Id == @event.InventoryItemId)
            .Decrease(@event.Quantity);

    private void When(DomainEvent.InventoryAdjustmentIncreased @event)
        => _items
            .Single(item => item.Id == @event.InventoryItemId)
            .Adjust(new IncreaseAdjustment(@event.Reason, @event.Quantity));

    private void When(DomainEvent.InventoryAdjustmentDecreased @event)
        => _items
            .Single(item => item.Id == @event.InventoryItemId)
            .Adjust(new DecreaseAdjustment(@event.Reason, @event.Quantity));

    // private void When(DomainEvent.StockDepleted _)
    // {
    //     Status = OutOfStock;
    // }

    private void When(DomainEvent.InventoryReserved @event)
        => _items
            .Single(item => item.Id == @event.InventoryItemId)
            .Reserve(@event.Quantity, @event.CartId, @event.Expiration);

    private void When(DomainEvent.InventoryNotReserved _) { }

    private string FormatSku(Product product)
    {
        var count = _items
            .Where(item => item.Product.Brand == product.Brand)
            .Where(item => item.Product.Category == product.Category)
            .Count(item => item.Product.Unit == product.Unit);

        return $"{product.Brand[..2]}{product.Category[..2]}{product.Unit[..2]}{count + 1}".ToUpperInvariant();
    }
}