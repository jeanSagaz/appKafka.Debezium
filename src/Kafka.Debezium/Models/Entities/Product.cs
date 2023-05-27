using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Entities
{
    public class Product
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Active")]
        public bool Active { get; set; }
    }
}
