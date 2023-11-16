using Application.DependencyInjection;
using CorrelationId;
using Domain.Abstractions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcService;
using GrpcService.Extensions;
using Infrastructure.EventBus.DependencyInjection.Extensions;
using Infrastructure.EventStore.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureLogging()
    .ConfigureServiceProvider()
    .ConfigureAppConfiguration()
    .ConfigureServices(services =>
    {
        // TODO: Improve this
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            options.Interceptors.Add<ErrorHandlingInterceptor>();
        });

        services
            // .AddProblemDetails(options => options.CustomizeProblemDetails = context =>
            // {
            //     if (context.Exception is IDomainException)
            //         context.ProblemDetails.Status = StatusCodes.Status400BadRequest;
            // })
            .AddCorrelationId();

        services
            .AddApplication()
            .AddMessageBusInfrastructure()
            .AddEventStoreInfrastructure();
    });

var app = builder.Build();

// app.UseExceptionHandler(applicationBuilder => applicationBuilder.Run(httpContext =>
// {
//     var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
//     Log.Error("[Exception Handler] {Error}", exception?.Message);
//
//     return httpContext.Response.WriteAsJsonAsync(
//         exception is IDomainException
//             ? ProblemDetail(StatusCodes.Status400BadRequest)
//             : ProblemDetail(StatusCodes.Status500InternalServerError));
//
//     ProblemDetails ProblemDetail(int status)
//     {
//         httpContext.Response.ContentType = "application/problem+json";
//         httpContext.Response.StatusCode = status;
//
//         return new()
//         {
//             Status = status,
//             Title = exception?.Message ?? "An error occurred",
//             Detail = exception?.InnerException?.Message ?? "An error occurred",
//             Instance = httpContext.Request.Path
//         };
//     }
// }));

app.UseCorrelationId();
app.UseSerilogRequestLogging();

app.MapGrpcService<CatalogingGrpcCommandService>();
app.MapHealthChecks("/healthz").ShortCircuit();

try
{
    if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        await app.MigrateEventStoreAsync();

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

namespace GrpcService
{
    public class ErrorHandlingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception exception) when (exception is IDomainException)
            {
                Log.Error("[Exception Handler] {Error}", exception.Message);
                var status = new Status(StatusCode.InvalidArgument, exception.Message);
                throw new RpcException(status);
            }
            catch (Exception exception)
            {
                Log.Error("[Exception Handler] {Error}", exception.Message);
                var status = new Status(StatusCode.Internal, exception.Message);
                throw new RpcException(status);
            }
        }
    }
}