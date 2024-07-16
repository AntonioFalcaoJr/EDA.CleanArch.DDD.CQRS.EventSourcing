using Fluxor;
using Refit;
using WebAPP.Store.Cataloging.Events;

namespace WebAPP.Store.Cataloging.Commands;

public record ChangeTitle(string CatalogId, string NewTitle, CancellationToken Token);

public class ChangeTitleReducer : Reducer<CatalogingState, ChangeTitle>
{
    public override CatalogingState Reduce(CatalogingState state, ChangeTitle action)
        => state with { IsEditingTitle = true };
}

public interface IChangeTitleApi
{
    [Put("/v1/catalogs/{catalogId}/title")]
    Task<IApiResponse> ChangeTitleAsync(string catalogId, [Body] string title, CancellationToken token);
}

public class ChangeTitleEffect(IChangeTitleApi api) : Effect<ChangeTitle>
{
    public override async Task HandleAsync(ChangeTitle cmd, IDispatcher dispatcher)
    {
        var response = await api.ChangeTitleAsync(cmd.CatalogId, cmd.NewTitle, cmd.Token);

        dispatcher.Dispatch(response.IsSuccessStatusCode
            ? new CatalogTitleChanged(cmd.CatalogId, cmd.NewTitle)
            : new CatalogTitleChangeFailed(cmd.CatalogId, response.Error?.Message ?? "Unknown error"));
    }
}