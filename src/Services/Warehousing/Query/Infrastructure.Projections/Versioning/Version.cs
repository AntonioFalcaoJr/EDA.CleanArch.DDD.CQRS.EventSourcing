namespace Infrastructure.Projections.Versioning;

public record Version
{
    private readonly ushort _value;
    
    private Version(ushort version)
    {
        _value = version;
    }
    
    private Version(int version)
    {
        VersionMustBePositive.ThrowIf(version < 0);
        _value = (ushort)version;
    }
    
    private Version(string version)
    {
        VersionFormatException
            .ThrowIf(!ushort.TryParse(version.Trim(), out _value));
    }

    public Version Next => new(_value + 1);

    public static Version Zero { get; } = new(0);
    public static Version Initial { get; } = new(1);
    public static Version Number(ushort version) => new(version);
    public static Version operator ++(Version version) => new(version._value + 1);

    public static explicit operator Version(string version) => new(version);
    public static explicit operator Version(ushort version) => new(version);
    public static implicit operator string(Version version) => version.ToString();
    public static implicit operator ushort(Version version) => version._value;
    
    public static bool operator <(Version left, Version right) => left._value < right._value;
    public static bool operator >(Version left, Version right) => left._value > right._value;
    public static bool operator %(Version left, Version right) => left._value % right._value == 0;

    public override string ToString() => _value.ToString();
}