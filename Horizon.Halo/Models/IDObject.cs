using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class IDObject
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }

    }
}
