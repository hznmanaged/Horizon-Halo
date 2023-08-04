using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class Team
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public String Name { get; set; }
        [JsonPropertyName("department_id")]
        public int DepartmentID { get; set; }
        [JsonPropertyName("ticket_count")]
        public int TicketCount { get; set; }
        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }
        [JsonPropertyName("inactive")]
        public bool Inactive { get; set; }

    }
}
