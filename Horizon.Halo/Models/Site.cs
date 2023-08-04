using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class Site
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        //[JsonPropertyName("client_id")]
        //public int ClientID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }
        [JsonPropertyName("timezone")]
        public string TimeZone { get; set; }
        [JsonPropertyName("inactive")]
        public bool Inactive { get; set; }
        [JsonPropertyName("client")]
        public Client Client { get; set; }
        [JsonPropertyName("customfields")]
        public IEnumerable<CustomField> CustomFields { get; set; }
    }
}
