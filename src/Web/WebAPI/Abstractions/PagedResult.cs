using Contracts.Abstractions.Paging;
using Google.Protobuf;
using PagedResult = Contracts.Abstractions.Protobuf.PagedResult;

namespace WebAPI.Abstractions;

public record PagedResult<TProjection> : IPagedResult<TProjection>
    where TProjection : IMessage, new()
{
    public IEnumerable<TProjection> Items { get; init; } = new List<TProjection>();
    public Page Page { get; init; } = new();

    public static implicit operator PagedResult<TProjection>(PagedResult result)
        => new()
        {
            Items = result.Projections.Select(proto => proto.Unpack<TProjection>()).ToList(),
            Page = result.Page
        };
}