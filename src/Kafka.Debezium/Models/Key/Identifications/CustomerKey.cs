using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key.Identifications
{
    public class CustomerKey
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }
    }
}
