using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using TravelInspiration.API.Shared.Domain;
using TravelInspiration.API.Shared.Persistence;

namespace TravelInspiration.API.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob(
    TravelInspirationDbContext dbContext,
    IPublisher publisher) : IJob
{
    private readonly TravelInspirationDbContext _dbContext = dbContext;
    private readonly IPublisher _publisher = publisher;

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(x => x.ProcessedOn == null)
            .OrderBy(x => x.OccurredOn)
            .Take(20)
            .ToArrayAsync();

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<DomainEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                ) ?? throw new ArgumentException("Event can't be deserialized.");

                await _publisher.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
            }

            message.ProcessedOn = DateTimeOffset.UtcNow;
        }
        
        await _dbContext.SaveChangesAsync();
    }
}
