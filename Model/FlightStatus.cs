using System.Text.Json.Serialization;

namespace FlightSystem.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FlightStatus
    {
        OnTime,
        Delayed,
        Cancelled,
        Boarding,
        InFlight,
    }
}