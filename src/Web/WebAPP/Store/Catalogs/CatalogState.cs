using System.Collections.Immutable;
using Contracts.Boundaries.Cataloging.Catalog;
using Fluxor;
using WebAPP.Abstractions;

namespace WebAPP.Store.Catalogs;

[FeatureState]
public record CatalogState
{
    public string CatalogId { get; init; } = "Undefined";
    public IImmutableList<CatalogItem> Items { get; init; } = ImmutableList<CatalogItem>.Empty;
    public bool IsFetching { get; init; }
    public bool HasError { get; init; }
    public string Error { get; init; } = string.Empty;
    public Page Page { get; init; } = new();
}

public record CatalogItem
{
    public string ItemId { get; init; } = "Undefined";
    public string ProductId { get; init; } = "Undefined";
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public static implicit operator CatalogItem(Projection.CatalogItemListItem item)
        => new()
        {
            ItemId = item.Id,
            ProductId = item.ProductId,
            Name = item.Product.Name,
            Description = "Description static on client side" // TODO: solve this
        };
}