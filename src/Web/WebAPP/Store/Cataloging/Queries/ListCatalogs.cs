using Fluxor;
using Refit;
using WebAPP.Abstractions;
using WebAPP.Store.Cataloging.Events;

namespace WebAPP.Store.Cataloging.Queries;

public record ListCatalogs(Paging Paging, CancellationToken CancellationToken);

public interface IListCatalogsApi
{
    [Get("/v1/catalogs")]
    Task<IApiResponse<IPagedResult<Catalog>>> ListAsync([Query] Paging paging, CancellationToken cancellationToken);
}

public class ListCatalogsEffect(IListCatalogsApi api) : Effect<ListCatalogs>
{
    public override async Task HandleAsync(ListCatalogs query, IDispatcher dispatcher)
    {
        var response = await api.ListAsync(query.Paging, query.CancellationToken);

        dispatcher.Dispatch(response is { IsSuccessStatusCode: true, Content: not null }
            ? new CatalogsListed(response.Content)
            : new CatalogsListingFailed(response.Error?.Message ?? "Unknown error"));
    }
}