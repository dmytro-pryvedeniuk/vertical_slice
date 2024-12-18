using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Itineraries;

public sealed class GetItineraries : ISlice
{
    private static AuthorizationPolicy HasGetItinerariesFeaturePolicy =>
        new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("feature", "get-itineraries")
            .Build();

    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/itineraries",
            (string? searchFor,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetItinerariesQuery(searchFor);
            return mediator.Send(query, cancellationToken);
        }).RequireAuthorization(HasGetItinerariesFeaturePolicy);
    }

    public record GetItinerariesQuery(string? SearchFor)
        : IRequest<IResult>
    {
    }

    public sealed class GetItinerariesHandler(
        TravelInspirationDbContext dbContext,
        IMapper mapper)
        : IRequestHandler<GetItinerariesQuery, IResult>
    {
        private readonly IMapper _mapper = mapper;
        private readonly TravelInspirationDbContext _dbContext = dbContext;

        public async Task<IResult> Handle(
            GetItinerariesQuery request,
            CancellationToken cancellationToken)
        {
            var searchFor = request.SearchFor;
            var itinerariesFromDb = await _dbContext.Itineraries.Where(
                i => searchFor == null
                    || i.Name.Contains(searchFor)
                    || (i.Description != null && i.Description.Contains(searchFor)))
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            var itineraries = _mapper.Map<IEnumerable<ItineraryDto>>(itinerariesFromDb);
            return Results.Ok(itineraries);
        }
    }

    public class ItineraryDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string UserId { get; set; }
    }

    class ItineraryMapProfile : Profile
    {
        public ItineraryMapProfile()
        {
            CreateMap<Itinerary, ItineraryDto>();
        }
    }
}
