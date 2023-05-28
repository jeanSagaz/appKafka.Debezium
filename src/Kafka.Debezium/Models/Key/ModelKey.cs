using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key
{
    public class ModelKey<KeyEntity>
    {
        [JsonPropertyName("payload")]
        public KeyEntity? Payload { get; set; }

        [JsonPropertyName("schema")]
        public SchemaPrimary? Schema { get; set; }
    }
}
