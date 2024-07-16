using System.Collections.Immutable;
using Fluxor;

namespace WebAPP.Store.Cataloging.Events;

public record ProductsSearchEmpty;

public class ProductsSearchEmptyReducer : Reducer<CatalogingState, ProductsSearchEmpty>
{
    public override CatalogingState Reduce(CatalogingState state, ProductsSearchEmpty _)
        => state with { IsSearching = false, Products = ImmutableList<Product>.Empty };
}