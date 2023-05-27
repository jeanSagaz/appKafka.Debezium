using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models
{
    public class FieldsPrimary
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }        

        [JsonPropertyName("optional")]
        public bool Optional { get; set; }

        [JsonPropertyName("field")]
        public string? Field { get; set; }
    }
}
