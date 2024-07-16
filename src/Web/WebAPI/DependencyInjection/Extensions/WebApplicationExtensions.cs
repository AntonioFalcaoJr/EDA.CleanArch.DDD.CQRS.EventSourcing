using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI.DependencyInjection.Extensions;

public static class WebApplicationExtensions
{
    public static void ConfigureSwagger(this WebApplication webApp)
    {
        webApp.UseSwagger();
        
        webApp.UseSwaggerUI(options =>
        {
            foreach (var version in webApp.DescribeApiVersions().Select(version => version.GroupName))
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);

            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.EnableValidator();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        });

        webApp
            .MapGet("/", () => Results.Redirect("/swagger/index.html"))
            .ShortCircuit()
            .ExcludeFromDescription();
    }
}