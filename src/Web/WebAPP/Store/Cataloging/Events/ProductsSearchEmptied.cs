using System.Collections.Immutable;
using Fluxor;

namespace WebAPP.Store.Cataloging.Events;

public record ProductsSearchEmptied(CancellationToken CancellationToken);

public class ProductsSearchEmptiedReducer : Reducer<CatalogingState, ProductsSearchEmptied>
{
    public override CatalogingState Reduce(CatalogingState state, ProductsSearchEmptied _)
        => state with { IsSearching = false, Products = ImmutableList<Product>.Empty };
}