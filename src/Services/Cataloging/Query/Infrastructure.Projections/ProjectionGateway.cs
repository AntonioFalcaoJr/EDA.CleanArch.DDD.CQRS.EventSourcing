using System.Linq.Expressions;
using Application.Abstractions;
using Contracts.Abstractions;
using Contracts.Abstractions.Paging;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Projections;

public class ProjectionGateway<TProjection>(ElasticsearchClient client) : IProjectionGateway<TProjection>
    where TProjection : IProjection
{
    public Task<TProjection?> GetAsync<TId>(TId id, CancellationToken cancellationToken) 
        => FindAsync(projection => projection.Id.Equals(id), cancellationToken);

    public ValueTask<IPagedResult<TProjection>> ListAsync(Paging paging, Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IPagedResult<TProjection>> ListAsync(Paging paging, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask ReplaceInsertAsync(TProjection replacement, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask RebuildInsertAsync(TProjection replacement, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateFieldAsync<TField, TId>(TId id, ulong version, Expression<Func<TProjection, TField>> field, TField value,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TProjection>> SearchAsync(string fragment, Paging paging, CancellationToken cancellationToken)
    {
        var response = await client.SearchAsync<TProjection>(descriptor
                => descriptor.Query(query
                        => query.Match(match
                            => match.Query(fragment)))
                    .From(paging.Number)
                    .Size(paging.Size),
            cancellationToken).WaitAsync(cancellationToken);

        return response.Documents;
    }

    public Task IndexAsync(TProjection projection, CancellationToken cancellationToken) 
        => client.IndexAsync(projection, cancellationToken);

    public async Task<TProjection?> FindAsync(Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken)
    {
        var response = await client.SearchAsync<TProjection>(descriptor
                => descriptor.Query(query
                    => query.MatchAll()).Size(1),
            cancellationToken);

        return response.Documents.FirstOrDefault();
    }


}