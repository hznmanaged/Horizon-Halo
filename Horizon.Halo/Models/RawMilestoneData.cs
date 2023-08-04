using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    internal class RawMilestoneData: IDObject
    {
        [JsonPropertyName("milestones")]
        public IEnumerable<Dictionary<string, object>> Milestones { get; set; }
    }
}
