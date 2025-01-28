using Ardalis.SmartEnum;

namespace Domain.Enumerations;

public class EmailStatus(string name, int value) : SmartEnum<EmailStatus>(name, value)
{
    public static readonly Undefined Undefined = new();
    public static readonly EmailUnverified Unverified = new();
    public static readonly EmailVerified Verified = new();
    public static readonly EmailExpired Expired = new();

    public static explicit operator EmailStatus(int value) => FromValue(value);
    public static explicit operator EmailStatus(string name) => FromName(name);
    public static implicit operator int(EmailStatus status) => status.Value;
    public static implicit operator string(EmailStatus status) => status.Name;
    public override string ToString() => Name;
}

public class Undefined() : EmailStatus(nameof(Undefined), 0);
public class EmailUnverified() : EmailStatus(nameof(Unverified), 1);
public class EmailVerified() : EmailStatus(nameof(Verified), 2);
public class EmailExpired() : EmailStatus(nameof(Expired), 3);