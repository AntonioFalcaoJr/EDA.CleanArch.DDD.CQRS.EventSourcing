﻿using System.Linq.Expressions;
using Application.Abstractions.EventSourcing.Projections;
using Application.EventSourcing.Projections;
using ECommerce.Abstractions.Messages.Queries.Paging;

namespace Infrastructure.Projections;

public class OrderProjectionsService : IOrderProjectionsService
{
    private readonly IOrderProjectionsRepository _repository;

    public OrderProjectionsService(IOrderProjectionsRepository repository)
    {
        _repository = repository;
    }

    public Task<IPagedResult<OrderDetailsProjection>> GetOrderDetailsWithPaginationAsync(IPaging paging, Expression<Func<OrderDetailsProjection, bool>> predicate, CancellationToken cancellationToken)
        => _repository.GetAllAsync(paging, predicate, cancellationToken);

    public Task<OrderDetailsProjection> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken)
        => _repository.GetAsync<OrderDetailsProjection, Guid>(orderId, cancellationToken);

    public Task ProjectAsync<TProjection>(TProjection projection, CancellationToken cancellationToken)
        where TProjection : IProjection
        => _repository.UpsertAsync(projection, cancellationToken);

    public Task RemoveAsync<TProjection>(Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken)
        where TProjection : IProjection
        => _repository.DeleteAsync(filter, cancellationToken);
}