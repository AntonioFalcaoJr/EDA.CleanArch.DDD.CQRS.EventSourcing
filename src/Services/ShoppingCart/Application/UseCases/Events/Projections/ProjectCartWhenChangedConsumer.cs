﻿using Application.Abstractions.Projections;
using Contracts.Services.ShoppingCart;
using Domain.Enumerations;
using MassTransit;

namespace Application.UseCases.Events.Projections;

public class ProjectCartWhenChangedConsumer :
    IConsumer<DomainEvent.BillingAddressChanged>,
    IConsumer<DomainEvent.CartCreated>,
    IConsumer<DomainEvent.CartItemAdded>,
    IConsumer<DomainEvent.CartItemRemoved>,
    IConsumer<DomainEvent.CartCheckedOut>,
    IConsumer<DomainEvent.ShippingAddressAdded>,
    IConsumer<DomainEvent.CartItemIncreased>,
    IConsumer<DomainEvent.CartItemDecreased>,
    IConsumer<DomainEvent.CartDiscarded>
{
    private readonly IProjectionRepository<Projection.ShoppingCart> _repository;

    public ProjectCartWhenChangedConsumer(IProjectionRepository<Projection.ShoppingCart> repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<DomainEvent.BillingAddressChanged> context)
        => await _repository.UpdateFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Customer.BillingAddress,
            value: context.Message.Address,
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartCheckedOut> context)
        => await _repository.UpdateFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Status,
            value: ShoppingCartStatus.CheckedOut.ToString(),
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartCreated> context)
    {
        var customer = new Projection.Customer(context.Message.CustomerId);
        var shoppingCart = new Projection.ShoppingCart(context.Message.CartId, customer, context.Message.Status);
        await _repository.InsertAsync(shoppingCart, context.CancellationToken);
    }

    public async Task Consume(ConsumeContext<DomainEvent.CartDiscarded> context)
        => await _repository.DeleteAsync(context.Message.CartId, context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartItemAdded> context)
        => await _repository.IncreaseFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Total,
            value: context.Message.Quantity * context.Message.Product.UnitPrice,
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartItemDecreased> context)
        => await _repository.IncreaseFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Total,
            value: context.Message.UnitPrice * -1,
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartItemIncreased> context)
        => await _repository.IncreaseFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Total,
            value: context.Message.UnitPrice,
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.CartItemRemoved> context)
        => await _repository.IncreaseFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Total,
            value: context.Message.UnitPrice * context.Message.Quantity * -1,
            cancellationToken: context.CancellationToken);

    public async Task Consume(ConsumeContext<DomainEvent.ShippingAddressAdded> context)
        => await _repository.UpdateFieldAsync(
            id: context.Message.CartId,
            field: cart => cart.Customer.ShippingAddress,
            value: context.Message.Address,
            cancellationToken: context.CancellationToken);
}