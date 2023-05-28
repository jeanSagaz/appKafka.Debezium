using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key.Identifications
{
    public class ProductKey
    {
        [JsonPropertyName("Code")]
        public int Code { get; set; }
    }
}
