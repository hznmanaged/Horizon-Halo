using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ProjectMilestone: IDObject
    {
        [JsonPropertyName("ticket_id")]
        public int TicketID { get; set; }
        [JsonPropertyName("sequence")]
        public int? Sequence { get; set; }
        [JsonPropertyName("state")]
        public int State { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("target_date")]
        public DateTime? TargetDate { get; set; }

        [JsonPropertyName("tickets")]
        public IEnumerable<ProjectMilestoneTicket> Tickets { get; set; }
        [JsonPropertyName("tickets_list")]
        public IList<IDObject> TicketsList { get; set; }

        public ProjectMilestone()
        {
            Tickets = new HashSet<ProjectMilestoneTicket>();
            TicketsList = new List<IDObject>();
        }
    }

}
