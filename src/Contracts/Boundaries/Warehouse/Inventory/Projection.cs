using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Warehouse.Inventory;

public static class Projection
{
    public record InventoryGridItem(string Id, Guid OwnerId, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Warehouse.Protobuf.InventoryGridItem(InventoryGridItem inventoryGridItem)
            => new()
            {
                InventoryId = inventoryGridItem.Id,
                OwnerId = inventoryGridItem.OwnerId.ToString()
            };
    }

    public record InventoryItemListItem(string Id, Guid InventoryId, Dto.Product Product, int Quantity, string Sku, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Warehouse.Protobuf.InventoryItemListItem(InventoryItemListItem item)
            => new()
            {
                ItemId = item.Id,
                InventoryId = item.InventoryId.ToString(),
                Product = item.Product,
                Sku = item.Sku,
                Quantity = item.Quantity
            };
    }
}