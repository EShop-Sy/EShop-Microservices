using Aspire.Hosting.Yarp;
using Yarp.ReverseProxy.Transforms;

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
                    // .WithTransformForwarded(useHost: true, useProto: true, forFormat: NodeFormat.IpAndPort, byFormat: NodeFormat.Random, action: ForwardedTransformActions.Append)
                    // .WithTransformRequestHeader("X-Forwarded-Host", "gateway.eshop.sy.com")
                    // .WithTransformRequestHeader("RequestHeader", "X-Forwarded-Proto")
                    // .WithTransformRequestHeader("Set", "https")
                    .WithTransformXForwarded(
                        // headerPrefix: ForwardedTransformActions.Remove,
                        xDefault: ForwardedTransformActions.Set,
                        xFor: ForwardedTransformActions.Append,
                        xHost: ForwardedTransformActions.Append,
                        xProto: ForwardedTransformActions.Off,
                        xPrefix: ForwardedTransformActions.Remove
                        // ForwardedTransformActions? xFor = null,
                        // ForwardedTransformActions? xHost = null,
                        // ForwardedTransformActions? xProto = null,
                        // ForwardedTransformActions? xPrefix = null
                    )
                    .WithTransformResponseHeader("X-Powered-By", "YARP");
            }
        });
        return builder;
    }
}
