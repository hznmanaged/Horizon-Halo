using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo
{
    public class TicketField
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("rtid")]
        public int RTID { get; set; }
        [JsonPropertyName("fieldid")]
        public int FieldID { get; set; }
        [JsonPropertyName("seq")]
        public int Sequence { get; set; }
        [JsonPropertyName("tableid")]
        public int TableID { get; set; }
        [JsonPropertyName("groupid")]
        public int GroupID { get; set; }
        [JsonPropertyName("fieldname")]
        public string FieldName { get; set; }

        [JsonPropertyName("fieldinfo")]
        public TicketFieldInfo FieldInfo{ get; set; }
    }
}
