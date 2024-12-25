using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TravelInspiration.API.IntegrationTests.Factories;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.IntegrationTests.Fixtures;

public sealed class SliceFixture
{
    private readonly TravelInspirationWebApplicationFactory _factory;
    private static readonly Lock _lock = new();
    private static bool _dbInitialized = false;
    private IServiceScope? _scope;

    public IServiceScopeFactory ServiceScopeFactory { get; }

    public SliceFixture()
    {
        _factory = new TravelInspirationWebApplicationFactory();
        ServiceScopeFactory = _factory.Services
            .GetRequiredService<IServiceScopeFactory>();

        lock (_lock)
        {
            if (!_dbInitialized)
            {
                using var scope = ServiceScopeFactory.CreateScope();
                using var context = CreateContext(scope);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Stops.RemoveRange([.. context.Stops]);
                context.Itineraries.RemoveRange([.. context.Itineraries]);
                context.OutboxMessages.RemoveRange([.. context.OutboxMessages]);

                context.SaveChanges();

                _dbInitialized = true;
            }
        }
    }

    private static TravelInspirationDbContext CreateContext(IServiceScope serviceScope)
    {
        return serviceScope.ServiceProvider.GetRequiredService<TravelInspirationDbContext>();
    }

    public async Task ExecuteInTransactionAsync(
        Func<TravelInspirationDbContext, Task> action)
    {
        using (_scope = ServiceScopeFactory.CreateScope())
        {
            using var context = CreateContext(_scope);
            await context.Database.BeginTransactionAsync();

            await action(context);

            await context.Database.RollbackTransactionAsync();
        }
    }

    public async Task SendAsync(IRequest<IResult> commandOrQuery)
    {
        if (_scope is null)
            throw new ArgumentException("Must be called in the context of scope");

        var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(commandOrQuery);
    }
}
