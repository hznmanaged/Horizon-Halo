using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class Client
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("inactive")]
        public bool inactive { get; set; }
        [JsonPropertyName("customfields")]
        public IEnumerable<CustomField> CustomFields { get; set; }

    }
}
