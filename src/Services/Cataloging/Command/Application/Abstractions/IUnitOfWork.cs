namespace Application.Abstractions;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<CancellationToken, Task> operationAsync, CancellationToken cancellationToken);
}