using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ProjectBudget : IDObject
    {
        [JsonPropertyName("ticket_id")]
        public int TicketID { get; set; }

        [JsonPropertyName("budgettype_id")]
        public int BudgetTypeID{ get; set; }

        [JsonPropertyName("hours")]
        public decimal? Hours { get; set; }
        [JsonPropertyName("rate")]
        public decimal? Rate{ get; set; }


        public ProjectBudget()
        {
        }
    }

}
