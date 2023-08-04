using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class TicketType
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public String Name { get; set; }
        [JsonPropertyName("use")]
        public String Use { get; set; }
        [JsonPropertyName("ticket_count")]
        public int TicketCount { get; set; }
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("fields")]
        public IEnumerable<TicketField> Fields{ get; set; }

    }
}
