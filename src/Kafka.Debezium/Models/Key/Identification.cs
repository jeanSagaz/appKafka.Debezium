using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Key
{
    public class Identification
    {
        [JsonPropertyName("Id")]
        //public Guid? Id { get; set; }
        public object? Id { get; set; }
    }
}
