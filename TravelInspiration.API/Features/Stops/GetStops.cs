using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class GetStops : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("api/itineraries/{itineraryId}/stops",
            (int itineraryId,
            IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                var query = new GetStopsQuery(itineraryId);
                return mediator.Send(query, cancellationToken);
            }).RequireAuthorization();
    }

    public record GetStopsQuery(int ItineraryId) : IRequest<IResult>
    {
    }

    public sealed class GetStopsHandler(
        TravelInspirationDbContext dbContext,
        IMapper mapper)
        : IRequestHandler<GetStopsQuery, IResult>
    {
        private readonly IMapper _mapper = mapper;
        private readonly TravelInspirationDbContext _dbContext = dbContext;

        public async Task<IResult> Handle(
            GetStopsQuery request,
            CancellationToken cancellationToken)
        {
            var itinerary = await _dbContext.Itineraries
                .Include(i => i.Stops)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.ItineraryId, cancellationToken);

            if (itinerary is null)
                return Results.NotFound();

            var stops = _mapper.Map<IEnumerable<StopDto>>(itinerary.Stops);
            return Results.Ok(stops);
        }
    }

    public sealed record StopDto(
        int Id,
        string Name,
        bool Suggested,
        Uri? ImageUri
    );

    public class StopProfile : Profile
    {
        public StopProfile()
        {
            CreateMap<Stop, StopDto>();
        }
    }
}
