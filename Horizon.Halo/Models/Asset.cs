using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class Asset
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("inventory_number")]
        public string InventoryNumber { get; set; }
        [JsonPropertyName("client_id")]
        public int ClientID { get; set; }
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }
        [JsonPropertyName("assettype_id")]
        public int AsssetTypeID { get; set; }
        [JsonPropertyName("assettype_name")]
        public string AssetTypeName { get; set; }
        [JsonPropertyName("site_id")]
        public int SiteID { get; set; }
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("ninjarmm_id")]
        public int? NinjaRMMID { get; set; }
        [JsonPropertyName("ninja_url")]
        public string NinjaURL { get; set; }
        [JsonPropertyName("fields")]
        public IEnumerable<AssetField> Fields { get; set; }

    }
}
