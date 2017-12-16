using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    public class AirMediaReceiverRequest
    {
        [JsonProperty("action")]
        public Action Action { get; set; }

        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("media")]
        public string Media { get; set; }
    }
}