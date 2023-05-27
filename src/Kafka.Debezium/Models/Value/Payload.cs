using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Value
{
    public class Payload<TEntity>
    {
        [JsonPropertyName("before")]
        public TEntity? Before { get; set; }

        [JsonPropertyName("after")]
        public TEntity? After { get; set; }

        [JsonPropertyName("op")]
        public string? Op { get; set; }

        [JsonPropertyName("ts_ms")]
        public double? TsMs { get; set; }

        [JsonPropertyName("source")]
        public Source? Source { get; set; }
    }
}
