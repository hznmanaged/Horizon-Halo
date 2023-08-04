using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ProjectSearchResponse
    {
        [JsonPropertyName("record_count")]
        public int RecordCount { get; set; }

        [JsonPropertyName("tickets")]
        public IEnumerable<ProjectTicket> Tickets { get; set; }
    }
}
