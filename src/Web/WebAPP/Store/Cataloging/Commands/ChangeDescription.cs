using Fluxor;
using Refit;
using WebAPP.Store.Cataloging.Events;

namespace WebAPP.Store.Cataloging.Commands;

public record ChangeDescription(string CatalogId, string NewDescription, CancellationToken CancellationToken);

public class ChangeDescriptionReducer : Reducer<CatalogingState, ChangeDescription>
{
    public override CatalogingState Reduce(CatalogingState state, ChangeDescription action)
        => state with { IsEditingDescription = true };
}

public interface IChangeDescriptionApi
{
    [Put("/v1/catalogs/{catalogId}/description")]
    Task<IApiResponse> ChangeDescriptionAsync(string catalogId, [Body] string description, CancellationToken cancellationToken);
}

public class ChangeDescriptionEffect(IChangeDescriptionApi api) : Effect<ChangeDescription>
{
    public override async Task HandleAsync(ChangeDescription cmd, IDispatcher dispatcher)
    {
        var response = await api.ChangeDescriptionAsync(cmd.CatalogId, cmd.NewDescription, cmd.CancellationToken);

        dispatcher.Dispatch(response.IsSuccessStatusCode
            ? new CatalogDescriptionChanged(cmd.CatalogId, cmd.NewDescription)
            : new CatalogDescriptionChangeFailed(cmd.CatalogId, response.Error?.Message ?? "Unknown error"));
    }
}