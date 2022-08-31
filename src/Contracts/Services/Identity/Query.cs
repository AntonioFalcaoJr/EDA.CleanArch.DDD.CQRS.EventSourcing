﻿using Contracts.Abstractions.Messages;

namespace Contracts.Services.Identity;

public static class Query
{
    public record GetUserAuthentication(Guid UserId) : Message(CorrelationId: UserId), IQuery;

    public record Login(string Email, string Password) : Message, IQuery;
}