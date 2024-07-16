using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class CatalogStatus(string name, int value) : SmartEnum<CatalogStatus>(name, value)
{
    public static readonly CatalogStatus Empty = new CatalogEmpty();
    public static readonly CatalogStatus Active = new CatalogActive();
    public static readonly CatalogStatus Inactive = new CatalogInactive();
    public static readonly CatalogStatus Discarded = new CatalogDiscarded();

    public static explicit operator CatalogStatus(int value) => FromValue(value);
    public static explicit operator CatalogStatus(string name) => FromName(name);
    public static implicit operator int(CatalogStatus catalogStatus) => catalogStatus.Value;
    public static implicit operator string(CatalogStatus catalogStatus) => catalogStatus.Name;
    public override string ToString() => Name;
}

public class CatalogEmpty() : CatalogStatus(nameof(Empty), 0);
public class CatalogActive() : CatalogStatus(nameof(Active), 1);
public class CatalogInactive() : CatalogStatus(nameof(Inactive), 2);
public class CatalogDiscarded() : CatalogStatus(nameof(Discarded), 3);