using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Horizon.Halo.Models
{
    public class Appointment
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("complete_status")]
        public int CompleteStatus { get; set; }


        [JsonPropertyName("client_id")]
        public int ClientID { get; set; }
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }


        [JsonPropertyName("site_id")]
        public int SiteID { get; set; }
        [JsonPropertyName("site_name")]
        public string SiteName { get; set; }

        [JsonPropertyName("user_id")]
        public int UserID { get; set; }
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonPropertyName("agent_id")]
        public int AgentID { get; set; }
        [JsonPropertyName("agent_name")]
        public string AgentName { get; set; }


        [JsonPropertyName("ticket_id")]
        public int? TicketID { get; set; }
        [JsonPropertyName("start_date")]
        public string StartDateString { get; set; }

        public DateTimeOffset StartDate { get
            {
                return DateTimeOffset.Parse(StartDateString, null, DateTimeStyles.AssumeUniversal).ToLocalTime();
            }
        }

        [JsonPropertyName("end_date")]
        public string EndDateString { get; set; }
        public DateTimeOffset EndDate
        {
            get
            {
                return DateTimeOffset.Parse(EndDateString, null, DateTimeStyles.AssumeUniversal).ToLocalTime();
            }
        }


        [JsonPropertyName("allday")]
        public bool AllDay { get; set; }

        [JsonPropertyName("is_private")]
        public bool IsPrivate { get; set; }
        [JsonPropertyName("is_task")]
        public bool IsTask { get; set; }

    }
}
