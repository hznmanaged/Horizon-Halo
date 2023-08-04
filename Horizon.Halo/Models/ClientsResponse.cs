using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class ClientsResponse: PaginatedResponse<Client>
    {

        [JsonPropertyName("clients")]
        public IEnumerable<Client> Clients { 
            get {
                return this.Records; }
            set {
                this.Records = value;
            }
        }

    }
}
