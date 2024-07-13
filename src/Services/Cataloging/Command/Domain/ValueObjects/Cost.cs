namespace Domain.ValueObjects;

public record Cost(Amount Amount, Currency Currency) : Price(Amount, Currency);