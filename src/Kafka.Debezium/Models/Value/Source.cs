using System.Text.Json.Serialization;

namespace Kafka.Debezium.Models.Value
{
    public class Source
    {
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("connector")]
        public string? Connector { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("ts_ms")]
        public double? TsMs { get; set; }

        [JsonPropertyName("snapshot")]
        public string? Snapshot { get; set; }

        [JsonPropertyName("db")]
        public string? DataBase { get; set; }

        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        [JsonPropertyName("table")]
        public string? Table { get; set; }

        [JsonPropertyName("change_lsn")]
        public string? ChangeLsn { get; set; }

        [JsonPropertyName("commit_lsn")]
        public string? CommitLsn { get; set; }

        [JsonPropertyName("event_serial_no")]
        public int EventSerialNo { get; set; }
    }
}
