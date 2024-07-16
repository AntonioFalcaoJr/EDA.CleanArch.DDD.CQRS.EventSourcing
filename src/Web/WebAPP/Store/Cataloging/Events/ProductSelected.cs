using Fluxor;

namespace WebAPP.Store.Cataloging.Events;

public record ProductSelected(Product SelectedProduct);

public class ProductSelectedReducer : Reducer<CatalogingState, ProductSelected>
{
    public override CatalogingState Reduce(CatalogingState state, ProductSelected @event)
        => state with
        {
            SelectedProduct = @event.SelectedProduct,
            IsSearching = false,
            Error = string.Empty
        };
}