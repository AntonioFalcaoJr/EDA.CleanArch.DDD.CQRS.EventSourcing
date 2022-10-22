﻿using Contracts.Abstractions.Messages;

namespace Contracts.Services.Catalog;

public static class Query
{
    public record GetCatalog(Guid CatalogId) : Message, IQuery;

    public record GetCatalogs(ushort Limit, ushort Offset) : Message, IQuery;

    public record GetCatalogItems(Guid CatalogId, ushort Limit, ushort Offset) : Message, IQuery;

    public record GetAllItems(ushort Limit, ushort Offset) : Message, IQuery;
}