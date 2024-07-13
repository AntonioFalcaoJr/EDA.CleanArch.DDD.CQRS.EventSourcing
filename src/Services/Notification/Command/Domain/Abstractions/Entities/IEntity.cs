namespace Domain.Abstractions.Entities;

public interface IEntity<out TId>
    where TId : notnull
{
    TId Id { get; }
    bool IsDeleted { get; }
}