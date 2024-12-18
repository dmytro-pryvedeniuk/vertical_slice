using TravelInspiration.API.Features.Stops;
using TravelInspiration.API.Shared.Domain.Entities;
using TravelInspiration.API.Shared.Domain.Events;

namespace TravelInspiration.API.UnitTests.Shared.Domain.Entities;

public sealed class StopTests : IDisposable
{
    private readonly CreateStop.CreateStopCommand _command;
    private readonly Stop _stop;

    public StopTests()
    {
        _command = new CreateStop.CreateStopCommand(12, "Kyiv", "http://x.com/y.png");
        _stop = new Stop("Test");
    }

    [Fact]
    public void WhenExecutingCreateStopCommand_WithItineraryId_StopItineraryIdMustMatch()
    {
        _stop.HandleCreateStopCommand(_command);

        Assert.Equal(_command.ItineraryId, _stop.ItineraryId);
    }

    [Fact]
    public void WhenExecutingCreateStopCommand_WithValidInput_OneStopCreatedEventMustBeAdded()
    {
        _stop.HandleCreateStopCommand(_command);

        Assert.Single(_stop.DomainEvents);
        Assert.IsType<StopCreatedEvent>(_stop.DomainEvents[0]);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
