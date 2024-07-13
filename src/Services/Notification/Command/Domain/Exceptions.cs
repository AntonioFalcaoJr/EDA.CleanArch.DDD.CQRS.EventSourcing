using Domain.Abstractions;

namespace Domain;

public static class Exceptions
{
    public class InvalidIdentifier() : DomainException<InvalidIdentifier>("Invalid identifier.");
    public class AggregateIsDeleted() : DomainException<AggregateIsDeleted>("Aggregate is deleted.");
    public class VersionMustBePositive() : DomainException<VersionMustBePositive>("Version must be a positive number.");
    public class VersionFormatException() : DomainException<VersionFormatException>("Version must be a positive number.");
}