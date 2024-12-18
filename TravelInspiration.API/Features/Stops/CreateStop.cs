using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Security;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class CreateStop : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/itineraries/{itineraryId}/stops",
            (int itineraryId,
            CreateStopCommand createStopCommand,
            IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                createStopCommand.ItineraryId = itineraryId;
                return mediator.Send(createStopCommand, cancellationToken);
            }).RequireAuthorization(AuthorizationPolicies.HasWriteActionPolicy);
    }

    public sealed class CreateStopCommand(
        int itineraryId,
        string name,
        string? imageUri)
        : IRequest<IResult>
    {
        public int ItineraryId { get; set; } = itineraryId;
        public string Name { get; } = name;
        public string? ImageUri { get; } = imageUri;
    }

    public sealed class CreateStopHandler(
        TravelInspirationDbContext dbContext,
        IMapper mapper)
        : IRequestHandler<CreateStopCommand, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Handle(CreateStopCommand request, CancellationToken cancellationToken)
        {
            var doesItineraryExists = await _dbContext.Itineraries.AnyAsync(
                i => i.Id == request.ItineraryId, cancellationToken);

            if (!doesItineraryExists)
                return Results.NotFound();

            var stop = new Stop(request.Name);
            stop.HandleCreateStopCommand(request);

            _dbContext.Stops.Add(stop);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Results.Created(
                $"api/itineraries/{stop.ItineraryId}/stops/{stop.Id}",
                _mapper.Map<StopDto>(stop));
        }
    }

    public record StopDto(
        int Id,
        string Name,
        int ItineraryId,
        string? ImageUri,
        bool? Suggested
    );

    public sealed class StopDtoAfterCreationProfile : Profile
    {
        public StopDtoAfterCreationProfile()
        {
            CreateMap<Stop, StopDto>();
        }
    }

    public class CreateStopValidator : AbstractValidator<CreateStopCommand>
    {
        public CreateStopValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(200)
                .NotEmpty();
            RuleFor(x => x.ImageUri)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.ImageUri))
                .WithMessage("ImageUri must be a valid Uri.");
        }
    }
}
