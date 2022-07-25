﻿using Application.EventStore;
using Contracts.Services.Identity;
using MassTransit;

namespace Application.UseCases.Commands;

public class DeleteUserConsumer : IConsumer<Command.Delete>
{
    private readonly IUserEventStoreService _eventStore;

    public DeleteUserConsumer(IUserEventStoreService eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task Consume(ConsumeContext<Command.Delete> context)
    {
        var user = await _eventStore.LoadAsync(context.Message.UserId, context.CancellationToken);
        user.Handle(context.Message);
        await _eventStore.AppendAsync(user, context.CancellationToken);
    }
}