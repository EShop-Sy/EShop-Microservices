using Aspire.Hosting.Yarp;

namespace AppHost.Extensions;

internal static class YarpResourceBuilderExtensions
{
    public static IResourceBuilder<YarpResource> WithSettings(this IResourceBuilder<YarpResource> builder,
        IDictionary<string, IResourceBuilder<IResourceWithServiceDiscovery>> clusters)
    {
        builder.WithConfiguration(yarp =>
        {
            foreach ((string name, IResourceBuilder<IResourceWithServiceDiscovery> endpoint) in clusters)
            {
                var cluster = yarp.AddCluster(endpoint);

                yarp.AddRoute($"/{name}/{{**catch-all}}", cluster)
                    .WithTransformPathRouteValues("/{**catch-all}")
                    // .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
                    .WithTransformResponseHeader("X-Powered-By", "YARP");
            }
        });
        return builder;
    }
}
