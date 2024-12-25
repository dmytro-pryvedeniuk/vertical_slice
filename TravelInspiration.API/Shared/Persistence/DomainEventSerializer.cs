using Newtonsoft.Json;
using TravelInspiration.API.Shared.Domain;

namespace TravelInspiration.API.Shared.Persistence;

public static class DomainEventSerializer
{
    private readonly static JsonSerializerSettings _serializer = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public static string Serialize(DomainEvent domainEvent)
    {
        return JsonConvert.SerializeObject(domainEvent, _serializer);
    }

    public static T Deserialize<T>(string value)
        where T : DomainEvent
    {
        return JsonConvert.DeserializeObject<T>(value, _serializer)
            ?? throw new ArgumentException("Event can't be deserialized.");
    }
}
