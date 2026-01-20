namespace AppHost.Extensions;

internal static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> WithReferences(this IResourceBuilder<ProjectResource> builder,
        List<IResourceBuilder<IResourceWithServiceDiscovery>> sources)
    {
        foreach (var source in sources)
        {
            builder.WithReference(source);
            builder.WaitFor(source);
        }

        return builder;
    }

    public static IResourceBuilder<ProjectResource> WithReferences(this IResourceBuilder<ProjectResource> builder,
        List<IResourceBuilder<IResourceWithConnectionString>> sources)
    {
        foreach (var source in sources)
        {
            builder.WithReference(source);
            builder.WaitFor(source);
        }

        return builder;
    }
}
