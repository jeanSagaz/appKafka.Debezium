using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key.Identifications
{
    public class ProductKey
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
    }
}
