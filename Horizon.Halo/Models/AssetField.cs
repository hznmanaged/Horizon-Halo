using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class AssetField
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string FieldName { get; set; }
        [JsonPropertyName("value")]
        public dynamic Value { get; set; }
    }
}
