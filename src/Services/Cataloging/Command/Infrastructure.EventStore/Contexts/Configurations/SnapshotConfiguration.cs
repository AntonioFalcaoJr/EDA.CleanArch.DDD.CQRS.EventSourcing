using Domain.Abstractions.Aggregates;
using Domain.Abstractions.EventStore;
using Domain.Abstractions.Identities;
using Domain.Aggregates.CatalogItems;
using Domain.Aggregates.Catalogs;
using Domain.Aggregates.Products;
using Infrastructure.EventStore.Contexts.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EventStore.Contexts.Configurations;

public class CatalogSnapshotConfiguration : SnapshotConfiguration<Catalog, CatalogId>;

public class CatalogItemSnapshotConfiguration : SnapshotConfiguration<CatalogItem, CatalogItemId>;

public class ProductSnapshotConfiguration : SnapshotConfiguration<Product, ProductId>;

public abstract class SnapshotConfiguration<TAggregate, TId> : IEntityTypeConfiguration<Snapshot<TAggregate, TId>>
    where TAggregate : AggregateRoot<TId>
    where TId : GuidIdentifier, new()
{
    public void Configure(EntityTypeBuilder<Snapshot<TAggregate, TId>> builder)
    {
        builder.ToTable($"{typeof(TAggregate).Name}Snapshots");

        builder.HasKey(snapshot => new { snapshot.Version, snapshot.AggregateId });

        builder
            .Property(snapshot => snapshot.Aggregate)
            .HasConversion<AggregateConverter<TAggregate, TId>>()
            .IsRequired();

        builder
            .Property(snapshot => snapshot.AggregateId)
            .HasConversion<IdentifierConverter<TId>>()
            .IsRequired();

        builder.Property(snapshot => snapshot.Timestamp)
            .IsRequired();

        builder
            .Property(snapshot => snapshot.Version)
            .HasConversion<VersionConverter>()
            .IsRequired();
    }
}