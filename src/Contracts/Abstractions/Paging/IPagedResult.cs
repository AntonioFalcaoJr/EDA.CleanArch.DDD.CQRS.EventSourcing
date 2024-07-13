using MassTransit;

namespace Contracts.Abstractions.Paging;

[ExcludeFromTopology]
public interface IPagedResult<out TProjection>
{
    IEnumerable<TProjection> Items { get; }
    Page Page { get; }
}