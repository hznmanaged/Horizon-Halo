using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("page_no")]
        public int PageNumber { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }

        public IEnumerable<T> Records { get; set; }

    }
}
