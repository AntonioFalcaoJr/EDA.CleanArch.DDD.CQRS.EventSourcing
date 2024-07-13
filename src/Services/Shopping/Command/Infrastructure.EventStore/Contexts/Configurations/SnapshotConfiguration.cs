using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Domain.Abstractions.Identities;
using Domain.Aggregates.Checkouts;
using Domain.Aggregates.Products;
using Domain.Aggregates.ShoppingCarts;
using Infrastructure.EventStore.Contexts.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Version = Domain.ValueObjects.Version;

namespace Infrastructure.EventStore.Contexts.Configurations;

public class CartSnapshotConfiguration : SnapshotConfiguration<ShoppingCart, CartId>;
public class CheckoutSnapshotConfiguration : SnapshotConfiguration<Checkout, CheckoutId>;
public class ProductSnapshotConfiguration : SnapshotConfiguration<Product, ProductId>;

public abstract class SnapshotConfiguration<TAggregate, TId> : IEntityTypeConfiguration<Snapshot<TAggregate, TId>>
    where TAggregate : AggregateRoot<TId>
    where TId : GuidIdentifier, new()
{
    public virtual void Configure(EntityTypeBuilder<Snapshot<TAggregate, TId>> builder)
    {
        builder.ToTable($"{typeof(TAggregate).Name}Snapshots");

        builder.HasKey(snapshot => new { snapshot.Version, snapshot.AggregateId });

        builder
            .Property(snapshot => snapshot.Aggregate)
            .HasConversion<AggregateConverter<TAggregate, TId>>()
            .IsRequired();

        builder
            .Property(snapshot => snapshot.AggregateId)
            .HasConversion<Guid>(id => id, guid => GuidIdentifier.New<TId>(guid))
            .IsRequired();

        builder.Property(snapshot => snapshot.Timestamp)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.Version)
            .HasConversion<ushort>(version => version, number => Version.Number(number))
            .IsRequired();
    }
}