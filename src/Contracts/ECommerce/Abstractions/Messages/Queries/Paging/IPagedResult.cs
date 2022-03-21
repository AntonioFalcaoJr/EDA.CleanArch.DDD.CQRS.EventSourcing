using MassTransit;

namespace ECommerce.Abstractions.Messages.Queries.Paging;

[ExcludeFromTopology]
public interface IPagedResult<out T>
{
    IEnumerable<T> Items { get; }
    PageInfo PageInfo { get; }
}