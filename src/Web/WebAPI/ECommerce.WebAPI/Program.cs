using System;
using System.Reflection;
using ECommerce.WebAPI.DependencyInjection.Observers;
using MassTransit;
using MassTransit.Definition;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();
// .AddApplicationFluentValidation();

builder.Services.AddLogging(loggingBuilder =>
{
    Log.Logger = new LoggerConfiguration().ReadFrom
        .Configuration(builder.Configuration)
        .CreateLogger();

    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});

builder.Services.AddSwaggerGen(options
    => options.SwaggerDoc(
        name: "v1",
        info: new()
        {
            Title = "WebAPI",
            Version = "v1"
        }));

builder.Services
    .AddMassTransit(cfg =>
    {
        cfg.SetKebabCaseEndpointNameFormatter();

        cfg.UsingRabbitMq((_, bus) =>
        {
            bus.Host(
                host: "192.168.100.9",
                host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

            bus.ConnectConsumeObserver(new LoggingConsumeObserver());
            bus.ConnectPublishObserver(new LoggingPublishObserver());
            bus.ConnectSendObserver(new LoggingSendObserver());
            
            // Account
            MapQueueEndpoint<Messages.Accounts.Commands.DefineProfessionalAddress>();
            MapQueueEndpoint<Messages.Accounts.Commands.DefineResidenceAddress>();
            MapQueueEndpoint<Messages.Accounts.Commands.DeleteAccount>();
            MapQueueEndpoint<Messages.Accounts.Commands.CreateAccount>();
            MapQueueEndpoint<Messages.Accounts.Commands.UpdateProfile>();
            MapQueueEndpoint<Messages.Accounts.Queries.GetAccountDetails>();
            MapQueueEndpoint<Messages.Accounts.Queries.GetAccountsDetailsWithPagination>();

            // Catalog
            MapQueueEndpoint<Messages.Catalogs.Commands.CreateCatalog>();
            MapQueueEndpoint<Messages.Catalogs.Commands.UpdateCatalog>();
            MapQueueEndpoint<Messages.Catalogs.Commands.DeleteCatalog>();
            MapQueueEndpoint<Messages.Catalogs.Commands.ActivateCatalog>();
            MapQueueEndpoint<Messages.Catalogs.Commands.DeactivateCatalog>();
            MapQueueEndpoint<Messages.Catalogs.Commands.AddCatalogItem>();
            MapQueueEndpoint<Messages.Catalogs.Commands.RemoveCatalogItem>();
            MapQueueEndpoint<Messages.Catalogs.Commands.UpdateCatalogItem>();
            MapQueueEndpoint<Messages.Catalogs.Queries.GetCatalogItemsDetailsWithPagination>();

            //Identity
            MapQueueEndpoint<Messages.Identities.Commands.RegisterUser>();
            MapQueueEndpoint<Messages.Identities.Commands.ChangeUserPassword>();
            MapQueueEndpoint<Messages.Identities.Commands.DeleteUser>();
            MapQueueEndpoint<Messages.Identities.Queries.GetUserAuthenticationDetails>();

            //Shopping Cart
            MapQueueEndpoint<Messages.ShoppingCarts.Commands.CreateCart>();
            MapQueueEndpoint<Messages.ShoppingCarts.Commands.AddCartItem>();
            MapQueueEndpoint<Messages.ShoppingCarts.Commands.RemoveCartItem>();
            MapQueueEndpoint<Messages.ShoppingCarts.Queries.GetShoppingCart>();
        });
    })
    .AddGenericRequestClient()
    .AddMassTransitHostedService();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce.WebAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints
    => endpoints.MapControllers());

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}

static void MapQueueEndpoint<TMessage>()
    where TMessage : class
    => EndpointConvention.Map<TMessage>(new Uri($"exchange:{ToKebabCaseString(typeof(TMessage))}"));

static string ToKebabCaseString(MemberInfo member)
    => KebabCaseEndpointNameFormatter.Instance.SanitizeName(member.Name);