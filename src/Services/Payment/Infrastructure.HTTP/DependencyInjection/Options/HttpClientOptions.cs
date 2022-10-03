﻿using System.ComponentModel.DataAnnotations;

namespace Infrastructure.HttpClients.DependencyInjection.Options;

public record CreditCardHttpClientOptions : HttpClientOptions;

public record DebitCardHttpClientOptions : HttpClientOptions;

public record PayPalHttpClientOptions : HttpClientOptions;

public abstract record HttpClientOptions
{
    [Required, Url]
    public string BaseAddress { get; init; }

    [Required, MinLength(5)]
    public string CancelEndpoint { get; init; }

    [Required, MinLength(5)]
    public string AuthorizeEndpoint { get; init; }

    [Required, MinLength(5)]
    public string RefundEndpoint { get; init; }

    [Required, MinLength(5)]
    public string CaptureEndpoint { get; init; }

    [Required, Timestamp]
    public TimeSpan OverallTimeout { get; init; }

    [Required, Range(2, 20)]
    public int RetryCount { get; init; }

    [Required, Range(2, 20)]
    public int SleepDurationPower { get; init; }

    [Required, Range(2, 20)]
    public int EachRetryTimeout { get; init; }

    [Required, Range(2, 20)]
    public int CircuitBreaking { get; init; }

    [Required, Timestamp]
    public TimeSpan DurationOfBreak { get; init; }
}