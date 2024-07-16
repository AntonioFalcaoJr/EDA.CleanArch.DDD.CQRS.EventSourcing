using Fluxor;

namespace WebAPP.Store.Cataloging.Events;

public record ProductsSearchFailed(string Error);

public class ProductsSearchFailedReducer : Reducer<CatalogingState, ProductsSearchFailed>
{
    public override CatalogingState Reduce(CatalogingState state, ProductsSearchFailed @event)
        => state with { IsSearching = false, Error = @event.Error };
}