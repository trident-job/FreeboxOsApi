using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    public class Capabilities
    {
        [JsonProperty("photo")]
        public bool Photo { get; set; }

        [JsonProperty("screen")]
        public bool Screen { get; set; }

        [JsonProperty("audio")]
        public bool Audio { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }
    }
}