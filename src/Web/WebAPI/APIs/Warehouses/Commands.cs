using Contracts.Boundaries.Warehouse.Inventory;
using Contracts.DataTransferObjects;
using MassTransit;
using WebAPI.Abstractions;
using WebAPI.APIs.Warehouses.Validators;

namespace WebAPI.APIs.Warehouses;

public static class Commands
{
    public record CreateInventory(IBus Bus, Guid OwnerId, CancellationToken CancellationToken)
        : Validatable<CreateInventoryValidator>, ICommand<Command.CreateInventory>
    {
        public Command.CreateInventory Command => new(Guid.NewGuid(), OwnerId);
    }

    public record ReceiveInventoryItem(IBus Bus, Guid InventoryId, Dto.Product Product, decimal Cost, int Quantity, CancellationToken CancellationToken)
        : Validatable<ReceiveInventoryItemValidator>, ICommand<Command.ReceiveInventoryItem>
    {
        public Command.ReceiveInventoryItem Command => new(InventoryId, Product, Cost, Quantity);
    }

    public record IncreaseInventoryAdjust(IBus Bus, Guid InventoryId, Guid ItemId, int Quantity, string Reason, CancellationToken CancellationToken)
        : Validatable<IncreaseInventoryAdjustValidator>, ICommand<Command.IncreaseInventoryAdjust>
    {
        public Command.IncreaseInventoryAdjust Command => new(InventoryId, ItemId, Quantity, Reason);
    }

    public record DecreaseInventoryAdjust(IBus Bus, Guid InventoryId, Guid ItemId, int Quantity, string Reason, CancellationToken CancellationToken)
        : Validatable<DecreaseInventoryAdjustValidator>, ICommand<Command.DecreaseInventoryAdjust>
    {
        public Command.DecreaseInventoryAdjust Command => new(InventoryId, ItemId, Quantity, Reason);
    }
}