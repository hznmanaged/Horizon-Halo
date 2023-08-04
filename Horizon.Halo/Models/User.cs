using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("name")]
        public string UserName { get; set; }
        [JsonPropertyName("client_id")]
        public int ClientID { get; set; }
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }
        [JsonPropertyName("site_id")]
        public decimal SiteID { get; set; }
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; }
        [JsonPropertyName("emailaddress")]
        public string EmailAddress { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; }       
    }
}
