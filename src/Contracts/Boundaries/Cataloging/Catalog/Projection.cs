using Contracts.Abstractions;
using Contracts.DataTransferObjects;

namespace Contracts.Boundaries.Cataloging.Catalog;

public static class Projection
{
    public record CatalogGridItem(string Id, string Title, string Description, string ImageUrl, bool IsActive, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Cataloging.Query.Protobuf.CatalogGridItem(CatalogGridItem catalog)
            => new()
            {
                CatalogId = catalog.Id,
                Title = catalog.Title,
                Description = catalog.Description,
                ImageUrl = catalog.ImageUrl,
                IsActive = catalog.IsActive
            };
    }

    public record CatalogItemListItem(string Id, string CatalogId, string ProductId, Dto.Product Product, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Cataloging.Query.Protobuf.CatalogItemListItem(CatalogItemListItem item)
            => new()
            {
                CatalogId = item.CatalogId,
                ItemId = item.Id,
                ProductName = item.Product.Name
            };
    }

    public record CatalogItemCard(string Id, string CatalogId, Dto.Product Product, Dto.Money Price, string ImageUrl, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Cataloging.Query.Protobuf.CatalogItemCard(CatalogItemCard item)
            => new()
            {
                CatalogId = item.CatalogId,
                ItemId = item.Id,
                Product = item.Product,
                Description = "item.Product.Description", // TODO
                ImageUrl = item.ImageUrl,
                UnitPrice = item.Price
            };
    }

    public record CatalogItemDetails(string Id, string CatalogId, Dto.Product Product, Dto.Money Price, string ImageUrl, bool IsDeleted, ulong Version) : IProjection
    {
        public static implicit operator Services.Cataloging.Query.Protobuf.CatalogItemDetails(CatalogItemDetails item)
            => new()
            {
                CatalogId = item.CatalogId,
                ItemId = item.Id,
                Product = item.Product,
                Description = "item.Product.Description", // TODO
                ImageUrl = item.ImageUrl,
                UnitPrice = item.Price
            };
    }
}