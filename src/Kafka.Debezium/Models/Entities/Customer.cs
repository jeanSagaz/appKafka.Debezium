using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Entities
{
    public class Customer
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        //[JsonPropertyName("BirthDate")]
        //public DateTime BirthDate { get; set; }
    }
}
