using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models
{
    public class SchemaPrimary
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("fields")]
        public ICollection<FieldsPrimary>? Fields { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
