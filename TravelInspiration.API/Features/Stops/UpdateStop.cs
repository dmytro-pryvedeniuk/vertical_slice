using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Security;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API.Features.Stops;

public sealed class UpdateStop : ISlice
{
    public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPut(
            "api/itineraries/{itineraryId}/stops/{stopId}",
            (int itineraryId,
            int stopId,
            UpdateStopCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
            {
                var commandToUse = command with
                {
                    ItineraryId = itineraryId,
                    StopId = stopId
                };

                return mediator.Send(commandToUse, cancellationToken);
            }).RequireAuthorization(AuthorizationPolicies.HasWriteActionPolicy);
    }


    public sealed record UpdateStopCommand(
        int ItineraryId,
        int StopId,
        string Name,
        bool? Suggested,
        string? ImageUri
    ) : IRequest<IResult>;

    public sealed class UpdateStopHandler(
        TravelInspirationDbContext dbContext,
        IMapper mapper)
        : IRequestHandler<UpdateStopCommand, IResult>
    {
        private readonly TravelInspirationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> Handle(
            UpdateStopCommand request,
            CancellationToken cancellationToken)
        {
            var stop = await _dbContext.Stops
                .FirstOrDefaultAsync(x =>
                    x.ItineraryId == request.ItineraryId && x.Id == request.StopId,
                    cancellationToken);

            if (stop is null)
                return Results.NotFound("Stop not found");

            stop.HandleUpdateStopCommand(request);
            _dbContext.Stops.Update(stop);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(_mapper.Map<StopDto>(stop));
        }
    }

    public class UpdateStopValidator : AbstractValidator<UpdateStopCommand>
    {
        public UpdateStopValidator()
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

    public sealed record StopDto
    (
        int Id,
        int ItineraryId,
        string Name,
        string? ImageUri,
        bool Suggested
    );

    public sealed class StopMapProfileAfterUpdate : Profile
    {
        public StopMapProfileAfterUpdate()
        {
            CreateMap<Stop, StopDto>();
        }
    }
}
