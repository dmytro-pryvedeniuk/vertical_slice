using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Destinations;

public sealed class SearchDestinations : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/destinations",
            async (
                string? searchFor,
                ILoggerFactory loggerFactory,
                CancellationToken cancellationToken,
                IDestinationSearchApiClient client
            ) =>
        {
            var logger = loggerFactory.CreateLogger(nameof(SearchDestinations));
            logger.LogInformation("SearchDestination is called.");

            var externalResult = await client
                .GetDestinationsAsync(searchFor, cancellationToken);
            var destinations = externalResult.Select(x =>
                new
                {
                    x.Name,
                    x.Description,
                    x.ImageUri
                });

            return Results.Ok(destinations);
        });
    }
}
