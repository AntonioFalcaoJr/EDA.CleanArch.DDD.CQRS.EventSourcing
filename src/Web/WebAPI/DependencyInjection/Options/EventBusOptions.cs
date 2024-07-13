using System.ComponentModel.DataAnnotations;

namespace WebAPI.DependencyInjection.Options;

public record EventBusOptions
{
    [Required] public required Uri ConnectionString { get; init; }
    [Required, Range(1, 10)] public int RetryLimit { get; init; }
    [Required, Range(typeof(TimeSpan), "00:00:01", "00:00:30")] public TimeSpan InitialInterval { get; init; }
    [Required, Range(typeof(TimeSpan), "00:00:01", "00:00:30")] public TimeSpan IntervalIncrement { get; init; }
}