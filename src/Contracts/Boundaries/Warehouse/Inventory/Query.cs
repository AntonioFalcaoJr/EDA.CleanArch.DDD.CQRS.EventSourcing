using Contracts.Abstractions.Messages;
using Contracts.Abstractions.Paging;

namespace Contracts.Boundaries.Warehouse.Inventory;

public static class Query
{
    public record ListInventoryGridItems(Paging Paging) : IQuery
    {
        public static implicit operator ListInventoryGridItems(Services.Warehouse.Protobuf.ListInventoryGridItemsRequest request)
            => new(request.Paging);

    }

    public record ListInventoryItemsListItems(Guid InventoryId, Paging Paging) : IQuery
    {
        public static implicit operator ListInventoryItemsListItems(Services.Warehouse.Protobuf.ListInventoryItemsListItemsRequest request)
            => new(new(request.InventoryId), request.Paging);
    }
}