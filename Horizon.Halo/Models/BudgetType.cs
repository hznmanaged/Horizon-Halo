using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class BudgetType: IDObject
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("defaultrate")]
        public decimal DefaultRate { get; set; }
    }
}
