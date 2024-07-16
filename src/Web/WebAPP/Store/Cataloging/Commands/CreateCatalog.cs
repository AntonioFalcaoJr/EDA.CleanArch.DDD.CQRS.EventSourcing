using Fluxor;
using Refit;
using WebAPP.Store.Cataloging.Events;

namespace WebAPP.Store.Cataloging.Commands;

public record Identifier(string Id);

public record CreateCatalog(Catalog NewCatalog, CancellationToken Token);

public class CreateCatalogReducer : Reducer<CatalogingState, CreateCatalog>
{
    public override CatalogingState Reduce(CatalogingState state, CreateCatalog action)
        => state with { IsCreating = true, Error = string.Empty };
}

public interface ICreateCatalogApi
{
    [Post("/v1/catalogs")]
    Task<IApiResponse<Identifier>> CreateAsync([Body] Catalog catalog, CancellationToken token);
}

public class CreateCatalogEffect(ICreateCatalogApi api) : Effect<CreateCatalog>
{
    public override async Task HandleAsync(CreateCatalog cmd, IDispatcher dispatcher)
    {
        var response = await api.CreateAsync(cmd.NewCatalog, cmd.Token);

        dispatcher.Dispatch(response is { IsSuccessStatusCode: true, Content.Id: not null }
            ? new CatalogCreated(cmd.NewCatalog with { CatalogId = response.Content.Id })
            : new CatalogCreationFailed(response.Error?.Message ?? "Unknown error"));
    }
}