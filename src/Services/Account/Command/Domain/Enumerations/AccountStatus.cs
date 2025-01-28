using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class AccountStatus(string name, int value) : SmartEnum<AccountStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly AccountPending Pending = new();
    public static readonly AccountActive Active = new();
    public static readonly AccountInactive Inactive = new();
    public static readonly AccountClosed Closed = new();

    public static explicit operator AccountStatus(int value) => FromValue(value);
    public static explicit operator AccountStatus(string name) => FromName(name);
    public static implicit operator int(AccountStatus status) => status.Value;
    public static implicit operator string(AccountStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : AccountStatus(nameof(Undefined), 0);
public class AccountPending() : AccountStatus(nameof(Pending), 1);
public class AccountActive() : AccountStatus(nameof(Active), 2);
public class AccountInactive() : AccountStatus(nameof(Inactive), 3);
public class AccountClosed() : AccountStatus(nameof(Closed), 4);