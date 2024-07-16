using System.Reflection;
using BlazorStrap;
using CorrelationId.DependencyInjection;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Settings.Configuration;
using WebAPP;
using WebAPP.DependencyInjection.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.ConfigureContainer(new DefaultServiceProviderFactory(new()
{
    ValidateScopes = builder.HostEnvironment.IsDevelopment(),
    ValidateOnBuild = builder.HostEnvironment.IsDevelopment()
}));

builder.Configuration
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables();

// Log.Logger = new LoggerConfiguration().ReadFrom
//     .Configuration(builder.Configuration)
//     .CreateLogger();
//
// builder.Logging.ClearProviders();
// builder.Logging.AddSerilog();

builder.Services.AddDefaultCorrelationId(options =>
{
    options.RequestHeader =
        options.ResponseHeader =
            options.LoggingScopeKey = "CorrelationId";
    options.UpdateTraceIdentifier = true;
    options.AddToLoggingScope = true;
});

builder.Services.AddApis();
builder.Services.AddBlazorStrap();

builder.Services.AddFluxor(options =>
{
    options.ScanAssemblies(typeof(Program).Assembly);
    options.UseReduxDevTools(rdt => rdt.Name = AppDomain.CurrentDomain.FriendlyName);
});

builder.Services.ConfigureOptions();

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