using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class TicketFieldInfo
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("labellong")]
        public string LabelLong{ get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
        [JsonPropertyName("hint")]
        public string Hint { get; set; }

        [JsonPropertyName("lookup")]
        public int Lookup { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("custom")]
        public int Custom { get; set; }
        [JsonPropertyName("usage")]
        public int Usage { get; set; }
        [JsonPropertyName("readonly")]
        public bool ReadOnly { get; set; }
        [JsonPropertyName("characterlimit")]
        public int CharacterLimit { get; set; }
        [JsonPropertyName("characterlimittype")]
        public int CharacterTypeLimit { get; set; }
        [JsonPropertyName("inputtype")]
        public int InputType { get; set; }
        [JsonPropertyName("values")]
        public IEnumerable<TicketFieldValue> Values { get; set; }



    }
}
