using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Free.FreeboxOsApi.Models;
using Jint;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Free.FreeboxOsApi
{
    internal delegate string StringTranform(string str);

    public class Api
    {
        private const string LoginPath = "/api/v3/login/";
        private const string ReceiversPath = "/api/v3/airmedia/receivers/";
        private const string DownloadsPath = "/api/v3/downloads/";


        private readonly HttpClient _httpClient;
        private readonly string _password;

        public Api(Uri uri, string password)
        {
            _password = password;
            _httpClient = new HttpClient {BaseAddress = uri};
            _httpClient.DefaultRequestHeaders.Add("X-FBX-FREEBOX0S", "1");
        }

        public async Task<bool> LoginAsync()
        {
            var challengeAndSalt = await GetChallengeAndSaltAsync();
            var password = ObfuscatePassword(_password, challengeAndSalt.Challenge, challengeAndSalt.Salt);

            var bodyList = new[]
            {
                new KeyValuePair<string, string>("password", password)
            };

            HttpContent bodyContent = new FormUrlEncodedContent(bodyList);
            var response = await _httpClient.PostAsync(LoginPath, bodyContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<Response<LoginResult>>(jsonString);
            return loginResponse.Success;
        }

        public async Task<IEnumerable<AirmediaResult>> ListAirmediaReceivers()
        {
            var jsonString = await _httpClient.GetStringAsync(ReceiversPath);
            var response = JsonConvert.DeserializeObject<Response<List<AirmediaResult>>>(jsonString);
            return response.Result;
        }

        public async Task<IEnumerable<Download>> ListDownloads()
        {
            var jsonString = await _httpClient.GetStringAsync(DownloadsPath);
            var response = JsonConvert.DeserializeObject<Response<List<Download>>>(jsonString);
            return response.Result;
        }

        public async Task<AddDownloadResult> AddDownload(Uri downloadUri)
        {
            return await AddDownload(new AddDownload {DownloadUrl = downloadUri});
        }

        public async Task<AddDownloadResult> AddDownload(Uri downloadUri, string username, string password)
        {
            return await AddDownload(new AddDownload
            {
                DownloadUrl = downloadUri,
                Username = username,
                Password = password
            });
        }

        private async Task<AddDownloadResult> AddDownload(AddDownload parameter)
        {
            var payload = new Dictionary<string, string>();
            var singleDownload = false;
            if (parameter.DownloadUrls != null && parameter.DownloadUrls.Any())
            {
                // TODO
            }
            else if (parameter.DownloadUrl != null)
            {
                singleDownload = true;
                payload["download_url"] = parameter.DownloadUrl.ToString();
            }
            else
            {
                throw new ArgumentException("No download url");
            }

            if (parameter.Username != null)
                payload["username"] = parameter.Username;

            if (parameter.Password != null)
                payload["password"] = parameter.Password;

            var response = await _httpClient.PostAsync(DownloadsPath + "add", new FormUrlEncodedContent(payload));
            var jsonResult = JObject.Parse(await response.Content.ReadAsStringAsync());
            var addDownloadResult = new AddDownloadResult
            {
                Success = jsonResult["success"].ToObject<bool>(),
                TaskId = new List<int>()
            };

            if (singleDownload)
                addDownloadResult.TaskId.Add(jsonResult["result"]["id"].ToObject<int>());
            else
                addDownloadResult.TaskId.AddRange(jsonResult["result"]["id"].ToObject<List<int>>());

            return addDownloadResult;
        }

        public async Task<bool> SendAirMediaReceiverRequest(string receiverName, AirMediaReceiverRequest request)
        {
            var payload = JsonConvert.SerializeObject
            (
                request,
                new JsonSerializerSettings
                {
                    Converters = new JsonConverter[] {new StringEnumConverter {CamelCaseText = true}}
                }
            );

            var stringContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ReceiversPath + receiverName, stringContent);
            dynamic result = JObject.Parse(await response.Content.ReadAsStringAsync());
            return result.success;
        }

        private async Task<ChallengeAndSalt> GetChallengeAndSaltAsync()
        {
            var jsonString = await _httpClient.GetStringAsync(LoginPath);
            var loginResponse = JsonConvert.DeserializeObject<Response<LoginResult>>(jsonString);
            var challenge = string.Empty;

            var engine = new Engine();
            engine.SetValue("unescape", (StringTranform) WebUtility.UrlDecode);
            challenge = loginResponse.Result.Challenge.Aggregate(challenge,
                (current, challengeEntry) => current + engine.Execute(challengeEntry).GetCompletionValue().ToString());

            return new ChallengeAndSalt {Challenge = challenge, Salt = loginResponse.Result.PasswordSalt};
        }

        private static string ObfuscatePassword(string password, string challenge, string salt)
        {
            using (var sha1 = SHA1.Create())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
                using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(hash))))
                {
                    return ByteArrayToHexadecimalString(hmac.ComputeHash(Encoding.UTF8.GetBytes(challenge)));
                }
            }
        }

        private static string ByteArrayToHexadecimalString(IEnumerable<byte> byteArray)
        {
            return string.Join("", byteArray.Select(b => b.ToString("x2")));
        }
    }
}