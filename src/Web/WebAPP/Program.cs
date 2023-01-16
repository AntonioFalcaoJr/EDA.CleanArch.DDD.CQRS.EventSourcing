using System.Reflection;
using BlazorStrap;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using WebAPP;
using WebAPP.DependencyInjection.Extensions;
using WebAPP.DependencyInjection.Options;
using WebAPP.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.ConfigureContainer(new DefaultServiceProviderFactory(new()
{
    ValidateScopes = builder.HostEnvironment.IsDevelopment(),
    ValidateOnBuild = true
}));

builder.Configuration
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

const string executionIdDescription = "CorrelationId";

builder.Services.AddDefaultCorrelationId(
    options =>
    {
        options.RequestHeader =
            options.ResponseHeader =
                options.LoggingScopeKey = executionIdDescription;
        options.UpdateTraceIdentifier = true;
        options.AddToLoggingScope = true;
    });

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddECommerceHttpClient();
builder.Services.AddBlazorStrap();

builder.Services.AddScoped<CatalogCardViewModel>();
builder.Services.AddScoped<CatalogItemViewModel>();
builder.Services.AddScoped<CatalogGridViewModel>();

builder.Services.ConfigureECommerceHttpClientOptions(
    builder.Configuration.GetSection(nameof(ECommerceHttpClientOptions)));

var host = builder.Build();

try
{
    await host.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
    await host.DisposeAsync();
}