using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class TicketFieldValue
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value2")]
        public string Value2 { get; set; }

        [JsonPropertyName("value2_bool")]
        public bool Value2Bool { get; set; }

        [JsonPropertyName("value3")]
        public string Value3 { get; set; }

        [JsonPropertyName("value3_bool")]
        public bool Value3Bool { get; set; }

        [JsonPropertyName("value4")]
        public string Value4 { get; set; }

        [JsonPropertyName("value4_bool")]
        public bool Value4Bool { get; set; }

        [JsonPropertyName("value5")]
        public string Value5 { get; set; }

        [JsonPropertyName("value5_bool")]
        public bool Value5Bool { get; set; }

        [JsonPropertyName("value6")]
        public string Value6 { get; set; }

        [JsonPropertyName("value6_bool")]
        public bool Value6Bool { get; set; }



    }
}
