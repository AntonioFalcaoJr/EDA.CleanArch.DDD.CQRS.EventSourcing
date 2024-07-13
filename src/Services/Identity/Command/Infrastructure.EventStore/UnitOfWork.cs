using Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.EventStore;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private readonly DatabaseFacade _database = dbContext.Database;

    public Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken token)
        => _database.CreateExecutionStrategy().ExecuteAsync(ct => ExecuteTransactionAsync(operationAsync, ct), token);

    private async Task ExecuteTransactionAsync(Func<CancellationToken, Task> operationAsync, CancellationToken token)
    {
        await using var transaction = await _database.BeginTransactionAsync(token);
        await operationAsync(token);
        await transaction.CommitAsync(token);
    }
}