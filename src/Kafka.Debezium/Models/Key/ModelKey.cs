using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key
{
    public class ModelKey
    {
        [JsonPropertyName("payload")]
        public Identification? Payload { get; set; }

        [JsonPropertyName("schema")]
        public SchemaPrimary? Schema { get; set; }
    }
}
