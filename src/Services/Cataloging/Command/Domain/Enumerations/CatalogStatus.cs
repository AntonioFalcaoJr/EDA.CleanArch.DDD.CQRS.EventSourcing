using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class CatalogStatus(string name, int value) : SmartEnum<CatalogStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly CatalogEmpty Empty = new();
    public static readonly CatalogActive Active = new();
    public static readonly CatalogInactive Inactive = new();
    public static readonly CatalogDiscarded Discarded = new();

    public static explicit operator CatalogStatus(int value) => FromValue(value);
    public static explicit operator CatalogStatus(string name) => FromName(name);
    public static implicit operator int(CatalogStatus status) => status.Value;
    public static implicit operator string(CatalogStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : CatalogStatus(nameof(Undefined), 0);
public class CatalogEmpty() : CatalogStatus(nameof(Empty), 1);
public class CatalogActive() : CatalogStatus(nameof(Active), 2);
public class CatalogInactive() : CatalogStatus(nameof(Inactive), 3);
public class CatalogDiscarded() : CatalogStatus(nameof(Discarded), 4);