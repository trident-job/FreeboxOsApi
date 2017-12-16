using System;
using System.Linq;
using System.Threading.Tasks;
using Free.FreeboxOsApi.Models;
using Microsoft.Extensions.Configuration;
using Xunit;
using Action = Free.FreeboxOsApi.Models.Action;

namespace Free.FreeboxOsApi.Tests
{
    public class ApiTest
    {
        public ApiTest()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json");
            builder.AddUserSecrets<ApiTest>();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        [Fact]
        public async Task AddDownload()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);

            var result =
                await api.AddDownload(
                    new Uri("http://ftp.free.org/mirrors/videolan/vlc/2.2.1/win32/vlc-2.2.1-win32.exe"));
            Assert.True(result.Success);
            Assert.True(result.TaskId.Count == 1);
            Assert.True(result.TaskId.Single() > 0);
        }

        [Fact]
        public async Task AddDownloadPasswordProtected()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);

            var result = await api.AddDownload(new Uri("https://httpbin.org/basic-auth/user/passwd"), "user", "passwd");
            Assert.True(result.Success);
            Assert.True(result.TaskId.Count == 1);
            Assert.True(result.TaskId.Single() > 0);
        }

        [Fact]
        public async Task BadLogin()
        {
            var api = new Api(new Uri(Configuration["uri"]), "pas-changer-assiette-pour-fromage");
            var loginResult = await api.LoginAsync();
            Assert.False(loginResult);
        }

        [Fact]
        public async Task GoodLogin()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);
        }

        [Fact]
        public async Task ListAirmedia()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);

            var airmediaResults = await api.ListAirmediaReceivers();
            Assert.True(airmediaResults.Any());
        }

        [Fact]
        public async Task ListDownloads()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);

            var downloads = await api.ListDownloads();
            Assert.True(downloads.Any());
        }

        [Fact]
        public async Task PlayMedia()
        {
            var api = new Api(new Uri(Configuration["uri"]), Configuration["password"]);
            var loginResult = await api.LoginAsync();
            Assert.True(loginResult);

            var airmediaResults = (await api.ListAirmediaReceivers()).ToList();
            Assert.True(airmediaResults.Any());

            var freeboxPlayer = airmediaResults.First(r => r.Name.Contains("Player"));
            var request = new AirMediaReceiverRequest
            {
                Action = Action.Start,
                Media = "https://www.hq.nasa.gov/alsj/a11/a11v.1092418-0354.avi",
                MediaType = MediaType.Video
            };
            var startResult = await api.SendAirMediaReceiverRequest(freeboxPlayer.Name, request);
            Assert.True(startResult);
        }
    }
}