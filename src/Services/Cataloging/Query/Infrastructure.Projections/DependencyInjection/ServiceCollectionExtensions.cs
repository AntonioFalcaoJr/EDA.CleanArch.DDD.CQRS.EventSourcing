using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uri = System.Uri;

namespace Infrastructure.Projections.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddProjections(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("Elasticsearch");
            // TODO: Decide if Elastic will have an options class or not
            var settings = new ElasticsearchClientSettings(new Uri(connectionString)); 
            return new ElasticsearchClient(settings);
        });
    }
}