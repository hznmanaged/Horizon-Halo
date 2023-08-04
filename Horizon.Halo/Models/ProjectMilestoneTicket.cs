using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ProjectMilestoneTicket
    {
        [JsonPropertyName("id")]
        public int? ID { get; set; }
        [JsonPropertyName("milestone_id")]
        public int MilestoneID { get; set; }
        [JsonPropertyName("ticket_id")]
        public int TicketID { get; set; }
    }
}
