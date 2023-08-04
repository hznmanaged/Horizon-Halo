using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class CustomField
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public System.Text.Json.JsonElement Value { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
