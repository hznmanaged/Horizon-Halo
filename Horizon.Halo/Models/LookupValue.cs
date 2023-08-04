using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class LookupValue
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("lookupid")]
        public int LookupID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
