using Application.Services;
using Domain.Aggregates.Catalogs;
using MediatR;

namespace Application.UseCases.Catalogs.Commands;

public record DeleteCatalog(CatalogId CatalogId) : IRequest;

public class DeleteCatalogInteractor(IApplicationService service) : IRequestHandler<DeleteCatalog>
{
    public async Task Handle(DeleteCatalog cmd, CancellationToken token)
    {
        var catalog = await service.LoadAggregateAsync<Catalog, CatalogId>(cmd.CatalogId, token);
        catalog.Delete();
        await service.AppendEventsAsync<Catalog, CatalogId>(catalog, token);
    }
}