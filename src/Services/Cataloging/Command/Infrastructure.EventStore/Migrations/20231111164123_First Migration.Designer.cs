﻿// <auto-generated />
using System;
using Infrastructure.EventStore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.EventStore.Migrations
{
    [DbContext(typeof(EventStoreDbContext))]
    [Migration("20231111164123_First Migration")]
    partial class FirstMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("EventStore")
                .HasAnnotation("ProductVersion", "9.0.0-alpha.1.23559.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Abstractions.EventStore.Snapshot<Domain.Aggregates.CatalogItems.CatalogItem, Domain.Aggregates.CatalogItems.CatalogItemId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aggregate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("CatalogItemSnapshots", "EventStore");
                });

            modelBuilder.Entity("Domain.Abstractions.EventStore.Snapshot<Domain.Aggregates.Catalogs.Catalog, Domain.Aggregates.Catalogs.CatalogId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aggregate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("CatalogSnapshots", "EventStore");
                });

            modelBuilder.Entity("Domain.Abstractions.EventStore.Snapshot<Domain.Aggregates.Products.Product, Domain.Aggregates.Products.ProductId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Aggregate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("ProductSnapshots", "EventStore");
                });

            modelBuilder.Entity("Domain.Abstractions.EventStore.StoreEvent<Domain.Aggregates.CatalogItems.CatalogItem, Domain.Aggregates.CatalogItems.CatalogItemId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("CatalogItemStoreEvents", "EventStore");
                });

            modelBuilder.Entity("Domain.Abstractions.EventStore.StoreEvent<Domain.Aggregates.Catalogs.Catalog, Domain.Aggregates.Catalogs.CatalogId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("CatalogStoreEvents", "EventStore");
                });

            modelBuilder.Entity("Domain.Abstractions.EventStore.StoreEvent<Domain.Aggregates.Products.Product, Domain.Aggregates.Products.ProductId>", b =>
                {
                    b.Property<uint>("Version")
                        .HasColumnType("bigint");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Version", "AggregateId");

                    b.ToTable("ProductStoreEvents", "EventStore");
                });
#pragma warning restore 612, 618
        }
    }
}
