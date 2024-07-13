using System.Net;
using System.Net.Mail;
using Application.Abstractions;
using Application.Abstractions.Gateways;
using Domain.ValueObjects;
using Infrastructure.SMTP.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.SMTP.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddEmailGateway(this IServiceCollection services)
        => services.AddTransient<INotificationGateway<Email>, EmailGateway>();
    
    public static void AddLazyFactory(this IServiceCollection services)
        => services.AddTransient(typeof(ILazy<>), typeof(LazyFactory<>));

    public static void AddSmtpClient(this IServiceCollection services)
        => services.AddScoped<SmtpClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<SmtpOptions>>().Value;
            return new(options.Host, options.Port)
            {
                Credentials = new NetworkCredential(options.Username, options.Password),
                EnableSsl = options.EnableSsl
            };
        });
    
    public static OptionsBuilder<SmtpOptions> ConfigureSmtpOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<SmtpOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}