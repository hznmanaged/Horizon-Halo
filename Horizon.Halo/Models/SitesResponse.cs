using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class SitesResponse: PaginatedResponse<Site>
    {

        [JsonPropertyName("sites")]
        public IEnumerable<Site> Sites{ 
            get {
                return this.Records; }
            set {
                this.Records = value;
            }
        }

    }
}
