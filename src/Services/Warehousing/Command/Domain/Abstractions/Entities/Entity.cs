namespace Domain.Abstractions.Entities;

public abstract class Entity<TId> : IEntity<TId>
    where TId : notnull, new()
{
    public TId Id { get; protected set; } = new();
    public bool IsDeleted { get; protected set; }

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => left.Id.Equals(right.Id);
    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !left.Id.Equals(right.Id);
    
    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id.Equals(entity.Id);
    public override int GetHashCode() => HashCode.Combine(Id);
}