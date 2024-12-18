namespace TravelInspiration.API.Shared.Slices;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapSliceEndpoints(
        this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var slices = endpointRouteBuilder.ServiceProvider.GetServices<ISlice>();
        foreach (var slice in slices)
            slice.AddEndpoint(endpointRouteBuilder);
        return endpointRouteBuilder;
    }
}
