using Fluxor;
using Refit;
using WebAPP.Abstractions;
using System.Net;
using WebAPP.Store.Cataloging.Events;

namespace WebAPP.Store.Cataloging.Queries;

public record SearchProducts(string Fragment, Paging Paging, CancellationToken CancellationToken);

public class SearchProductsReducer : Reducer<CatalogingState, SearchProducts>
{
    public override CatalogingState Reduce(CatalogingState state, SearchProducts query)
        => state with { IsSearching = true, Fragment = query.Fragment, SelectedProduct = new() };
}

public interface ISearchProductsApi
{
    [Get("/v1/products/search")]
    Task<IApiResponse<IPagedResult<Product>>> SearchAsync([Query] string fragment, [Query] Paging paging, CancellationToken cancellationToken);
}

public class SearchProductsEffect(ISearchProductsApi api) : Effect<SearchProducts>
{
    public override async Task HandleAsync(SearchProducts query, IDispatcher dispatcher)
    {
        var response = await api.SearchAsync(query.Fragment, query.Paging, query.CancellationToken);

        dispatcher.Dispatch(response switch
        {
            { StatusCode: HttpStatusCode.NoContent } => new ProductsSearchEmpty(),
            { IsSuccessStatusCode: true, Content: not null } => new ProductsSearchHit(response.Content),
            _ => new ProductsSearchFailed(response.Error?.Message ?? "Unknown error")
        });
    }
}