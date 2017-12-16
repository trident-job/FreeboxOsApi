using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    public class Download
    {
        [JsonProperty("rx_bytes")]
        public double RxBytes { get; set; }

        [JsonProperty("tx_bytes")]
        public double TxBytes { get; set; }

        [JsonProperty("download_dir")]
        public string DownloadDir { get; set; }

        [JsonProperty("archive_password")]
        public string ArchivePassword { get; set; }

        [JsonProperty("eta")]
        public int Eta { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("io_priority")]
        public string IoPriority { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("queue_pos")]
        public int QueuePosition { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("created_ts")]
        public int CreatedTs { get; set; }

        [JsonProperty("stop_ratio")]
        public int StopRatio { get; set; }

        [JsonProperty("tx_rate")]
        public int TxRate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tx_pct")]
        public int TxPct { get; set; }

        [JsonProperty("rx_pct")]
        public int RxPct { get; set; }

        [JsonProperty("rx_rate")]
        public int RxRate { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("size")]
        public object Size { get; set; }
    }
}