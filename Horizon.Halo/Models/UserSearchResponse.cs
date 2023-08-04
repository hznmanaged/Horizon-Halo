using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class UserSearchResponse
    {
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }

        [JsonPropertyName("users")]
        public IEnumerable<User> Users { get; set; }
    }
}
