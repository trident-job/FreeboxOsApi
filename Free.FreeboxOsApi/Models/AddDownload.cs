using System;
using System.Collections.Generic;

namespace Free.FreeboxOsApi.Models
{
    public class AddDownload
    {
        public Uri DownloadUrl { get; set; }
        public List<Uri> DownloadUrls { get; set; }
        public string DownloadDir { get; set; }
        public bool Recursive { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ArchivePassword { get; set; }
        public string Cookies { get; set; }
    }
}