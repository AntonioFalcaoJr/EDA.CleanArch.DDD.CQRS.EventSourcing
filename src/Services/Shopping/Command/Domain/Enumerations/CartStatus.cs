using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class CartStatus(string name, int value) : SmartEnum<CartStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly CartEmpty Empty = new();
    public static readonly CartOpen Open = new();
    public static readonly CartAbandoned Abandoned = new();
    public static readonly CartCheckedOut CheckedOut = new();

    public static explicit operator CartStatus(int value) => FromValue(value);
    public static explicit operator CartStatus(string name) => FromName(name);
    public static implicit operator int(CartStatus status) => status.Value;
    public static implicit operator string(CartStatus status) => status.Name;
    public override string ToString() => Name;
}
public class Undefined() : CartStatus(nameof(Undefined), 0);
public class CartEmpty() : CartStatus(nameof(Empty), 0);
public class CartOpen() : CartStatus(nameof(Open), 1);
public class CartAbandoned() : CartStatus(nameof(Abandoned), 2);
public class CartCheckedOut() : CartStatus(nameof(CheckedOut), 3);