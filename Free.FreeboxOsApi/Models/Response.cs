using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    public class Response<T> where T : class
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }
    }
}