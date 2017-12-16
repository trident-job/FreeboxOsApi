using Newtonsoft.Json;

namespace Free.FreeboxOsApi.Models
{
    internal class LoginResult
    {
        [JsonProperty("logged_in")]
        public bool LoggedIn { get; set; }

        [JsonProperty("challenge")]
        public string[] Challenge { get; set; }

        [JsonProperty("password_salt")]
        public string PasswordSalt { get; set; }
    }
}