using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ProjectTicket: Ticket
    {

        [JsonPropertyName("budgettype_id")]
        public int? BudgetTypeID { get; set; }
        [JsonPropertyName("projecttimebudget")]
        public decimal? ProjectTimeBudget { get; set; }

        

        [JsonPropertyName("milestones")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ProjectMilestone> Milestones { get; set; }
    }
}
