using System.Collections.Immutable;
using Fluxor;
using WebAPP.Abstractions;
using WebAPP.Store.Catalogs;

namespace WebAPP.Store.Cataloging;

[FeatureState]
public record CatalogingState
{
    public IImmutableList<Catalog> Catalogs { get; init; } = ImmutableList<Catalog>.Empty;
    public IImmutableList<Product> Products { get; set; } = ImmutableList<Product>.Empty;
    public Catalog NewCatalog { get; init; } = new();
    public CatalogItem NewItem { get; init; } = new();
    public Product SelectedProduct { get; init; } = new();
    public string CatalogId { get; set; } = "Undefined";
    public bool HasError => Error != string.Empty;
    public bool IsCreating { get; init; }
    public bool IsFetching { get; init; }
    public bool IsDeleting { get; init; }
    public bool IsEditingTitle { get; init; }
    public bool IsAddingItem { get; init; }
    public bool IsEditingDescription { get; init; }
    public bool IsCreatingItem { get; set; }
    public string Error { get; init; } = string.Empty;
    public Page Page { get; init; } = new();
    public bool IsSearching { get; set; }
    public string Fragment { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public bool IsRetrying { get; set; }
}

public record Catalog
{
    public bool IsActive { get; set; }
    public string CatalogId { get; set; } = "Undefined";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}