using Contracts.Abstractions;

namespace Application.Projections;

public record SearchableProduct(string Id, string Name, string Description, /*string[] Tags, string[] Categories,*/ bool IsDeleted, ulong Version) : IProjection;
