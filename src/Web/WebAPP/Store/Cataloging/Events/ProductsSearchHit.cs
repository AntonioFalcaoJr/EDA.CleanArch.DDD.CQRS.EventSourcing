using System.Collections.Immutable;
using Fluxor;
using WebAPP.Abstractions;

namespace WebAPP.Store.Cataloging.Events;

public record ProductsSearchHit(IPagedResult<Product> Products);

public class ProductsSearchHitReducer : Reducer<CatalogingState, ProductsSearchHit>
{
    public override CatalogingState Reduce(CatalogingState state, ProductsSearchHit @event)
        => state with
        {
            Products = @event.Products.Items.ToImmutableList(),
            IsSearching = false,
            Error = string.Empty
        };
}