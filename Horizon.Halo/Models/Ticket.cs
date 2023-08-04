using Horizon.Halo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class Ticket
    {
        public readonly static List<String> BuiltInFields = new List<string>()
        {
            "team_id",
            "agent_id",
            "site_id",
            "client_id",
            "urgency",
            "impact",
            "tickettype_id"
        };

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ID { get; set; }
        [JsonPropertyName("dateoccurred")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTimeOffset? DateOccured { get; set; }
        [JsonPropertyName("summary")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Summary { get; set; }
        [JsonPropertyName("details")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Details { get; set; }
        [JsonPropertyName("tickettype_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TicketTypeID { get; set; }
        [JsonPropertyName("team_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TeamID { get; set; }
        [JsonPropertyName("status_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? StatusID { get; set; }
        
        [JsonPropertyName("priority_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PriorityID { get; set; }

        [JsonPropertyName("parent_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ParentID { get; set; }

        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ClientID { get; set; }

        [JsonPropertyName("client_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ClientName { get; set; }

        [JsonPropertyName("site_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? SiteID { get; set; }

        [JsonPropertyName("site_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SiteName { get; set; }
        [JsonPropertyName("user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? UserID { get; set; }

        [JsonPropertyName("user_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserName { get; set; }

        [JsonPropertyName("startdate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? StartDate { get; set; }
        [JsonPropertyName("starttime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TimeSpan? StartTime { get; set; }
        [JsonPropertyName("targetdate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? TargetDate { get; set; }
        [JsonPropertyName("targettime")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TimeSpan? TargetTime { get; set; }

        [JsonPropertyName("deadlinedate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DeadlineDate { get; set; }

        [JsonPropertyName("budgettype_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string BudgetTypeName { get; set; }
        

        [JsonPropertyName("assets")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<Asset> Assets{ get; set; }
    }
}
