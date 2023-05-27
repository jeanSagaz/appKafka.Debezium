using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Value
{
    public class ModelValue<TEntity>
    {
        [JsonPropertyName("schema")]
        public SchemaPrimary? Schema { get; set; }

        [JsonPropertyName("payload")]
        public Payload<TEntity>? Payload { get; set; }
    }
}
