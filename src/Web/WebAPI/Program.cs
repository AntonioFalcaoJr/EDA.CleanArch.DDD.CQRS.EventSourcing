using System.Reflection;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;
using Contracts.Enumerations;
using Contracts.JsonConverters;
using CorrelationId;
using CorrelationId.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Grpc.Core;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;
using WebAPI.APIs.Accounts;
using WebAPI.APIs.Catalogs;
using WebAPI.APIs.Communications;
using WebAPI.APIs.Identities;
using WebAPI.APIs.Orders;
using WebAPI.APIs.Payments;
using WebAPI.APIs.Shopping;
using WebAPI.APIs.Warehouses;
using WebAPI.DependencyInjection.Extensions;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider((context, provider) =>
{
    provider.ValidateScopes =
        provider.ValidateOnBuild =
            context.HostingEnvironment.IsDevelopment();
});

builder.Configuration
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables();

builder.Logging.ClearProviders().AddSerilog();

builder.Host.UseSerilog((context, cfg)
    => cfg.ReadFrom.Configuration(context.Configuration));

builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    if (context.Exception is not RpcException exception) return;

    context.ProblemDetails.Status = exception.StatusCode switch
    {
        StatusCode.NotFound => StatusCodes.Status404NotFound,
        StatusCode.AlreadyExists => StatusCodes.Status409Conflict,
        StatusCode.PermissionDenied => StatusCodes.Status403Forbidden,
        StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
        StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
        StatusCode.DeadlineExceeded => StatusCodes.Status408RequestTimeout,
        StatusCode.Cancelled => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    context.ProblemDetails.Title = exception.Status.ToString();
    context.ProblemDetails.Detail = exception.Status.Detail;
    context.HttpContext.Response.StatusCode = context.ProblemDetails.Status.Value;
});

builder.Services.AddCors(options
    => options.AddDefaultPolicy(policyBuilder
        => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()));

builder.Services.AddDefaultCorrelationId(options =>
    options.UpdateTraceIdentifier =
        options.IncludeInResponse =
            options.AddToLoggingScope = true);

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<WebAPI.Program>();

// TODO - Review it!
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyTextJsonConverter());
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.Converters.Add(new SmartEnumNameConverter<Gender, int>());
});

builder.Services
    .AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwagger();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEventBus();
builder.Services.AddGrpcClients();
builder.Services.AddHealthChecks();

builder.Services.ConfigureOptions();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseCors();
app.UseCorrelationId();
app.UseSerilogRequestLogging();

app.NewVersionedApi("Accounts").MapAccountApiV1().MapAccountApiV2();
app.NewVersionedApi("Cataloging").MapCatalogApiV1().MapCatalogApiV2();
app.NewVersionedApi("Communications").MapCommunicationApiV1().MapCommunicationApiV2();
app.NewVersionedApi("Identities").MapIdentityApiV1().MapIdentityApiV2();
app.NewVersionedApi("Orders").MapOrderApiV1().MapOrderApiV2();
app.NewVersionedApi("Payments").MapPaymentApiV1().MapPaymentApiV2();
app.NewVersionedApi("Shopping").MapShoppingApiV1().MapShoppingApiV2();
app.NewVersionedApi("Warehousing").MapWarehouseApiV1().MapWarehouseApiV2();

app.MapHealthChecks("/healthz").ShortCircuit();

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

namespace WebAPI
{
    public partial class Program;
}