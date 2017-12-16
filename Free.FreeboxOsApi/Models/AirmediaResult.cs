using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    public class AirmediaResult
    {
        [JsonProperty("capabilities")]
        public Capabilities Capabilities { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password_protected")]
        public bool PasswordProtected { get; set; }
    }
}