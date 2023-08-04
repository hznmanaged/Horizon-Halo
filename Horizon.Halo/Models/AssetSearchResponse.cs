using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class AssetSearchResponse
    {
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }

        [JsonPropertyName("assets")]
        public IEnumerable<Asset> Assets { get; set; }
    }
}
