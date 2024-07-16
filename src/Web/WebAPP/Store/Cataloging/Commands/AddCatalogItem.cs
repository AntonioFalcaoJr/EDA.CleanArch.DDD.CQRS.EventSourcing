using Fluxor;
using Refit;
using WebAPP.Store.Cataloging.Events;
using WebAPP.Store.Catalogs;

namespace WebAPP.Store.Cataloging.Commands;

public record AddCatalogItem(string CatalogId, CatalogItem NewItem, CancellationToken Token);

public class AddCatalogItemReducer : Reducer<CatalogingState, AddCatalogItem>
{
    public override CatalogingState Reduce(CatalogingState state, AddCatalogItem action)
        => state with { IsAddingItem = true };
}

public interface IAddCatalogItemApi
{
    [Post("/v1/catalogs/{catalogId}/items")]
    Task<IApiResponse> AddCatalogItemAsync(string catalogId, [Body] CatalogItem item, CancellationToken token);
}

public class AddCatalogItemEffect(IAddCatalogItemApi api) : Effect<AddCatalogItem>
{
    public override async Task HandleAsync(AddCatalogItem cmd, IDispatcher dispatcher)
    {
        var response = await api.AddCatalogItemAsync(cmd.CatalogId, cmd.NewItem, cmd.Token);

        dispatcher.Dispatch(response.IsSuccessStatusCode
            ? new CatalogItemAdded(cmd.NewItem)
            : new CatalogItemAddingFailed((response.Error?.Message ?? response.ReasonPhrase)!));
    }
}