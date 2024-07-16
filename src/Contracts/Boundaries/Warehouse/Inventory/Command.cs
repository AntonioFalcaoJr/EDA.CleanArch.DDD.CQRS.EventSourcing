using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Warehouse.Inventory;

public static class Command
{
    public record ReceiveInventoryItem(Guid InventoryId, Dto.Product Product, decimal Cost, int Quantity) : Message, ICommand;

    public record IncreaseInventoryAdjust(Guid InventoryId, Guid ItemId, int Quantity, string Reason) : Message, ICommand;

    public record DecreaseInventoryAdjust(Guid InventoryId, Guid ItemId, int Quantity, string Reason) : Message, ICommand;

    public record ReserveInventoryItem(Guid InventoryId, Guid CatalogId, Guid CartId, Dto.Product Product, int Quantity) : Message, ICommand;

    public record CreateInventory(Guid InventoryId, Guid OwnerId) : Message, ICommand;
}