using System.Collections.Generic;

namespace Free.FreeboxOsApi.Models
{
    public class AddDownloadResult
    {
        public bool Success { get; set; }
        public List<int> TaskId { get; set; }
    }
}