using Contracts.Abstractions;
using Contracts.Abstractions.Paging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Projections.Pagination;

public record PagedResult<TProjection> : IPagedResult<TProjection> where TProjection : IProjection
{
    private readonly List<TProjection> _items;
    private readonly Paging _paging;
    
    private PagedResult(IEnumerable<TProjection> items, Paging paging)
    {
        _items = items.ToList();
        _paging = paging;
    }
    
    public IEnumerable<TProjection> Items 
        => Page.HasNext ? _items.Take(_paging.Size) : _items;

    public Page Page => new()
    {
        Number = _paging.Number,
        Size = _paging.Size,
        HasNext = _items.Count > _paging.Size,
        HasPrevious = _paging.Number > 1
    };

    public static async ValueTask<IPagedResult<TProjection>> CreateAsync(IQueryable<TProjection> source, Paging paging, CancellationToken token)
    {
        var projections = await ApplyPagination(source, paging).ToListAsync(token);
        return new PagedResult<TProjection>(projections, paging);
    }

    private static IMongoQueryable<TProjection> ApplyPagination(IQueryable<TProjection> source, Paging paging)
        => (IMongoQueryable<TProjection>)source.Skip(paging.Size * (paging.Number - 1)).Take(paging.Size + 1);
}