namespace Contracts.Abstractions;

public interface IProjection
{
    string Id { get; }
    bool IsDeleted { get; }
    ulong Version { get; }
}