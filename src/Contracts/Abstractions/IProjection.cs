﻿namespace Contracts.Abstractions;

public interface IProjection
{
    Guid Id { get; }
    bool IsDeleted { get; }
    ulong Version { get; }
}