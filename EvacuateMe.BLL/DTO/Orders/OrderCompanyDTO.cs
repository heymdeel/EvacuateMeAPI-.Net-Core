using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.DTO.Orders
{
    public class OrderCompanyDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("contact_phone")]
        public string ContactPhone { get; set; }

        [JsonProperty("email")]
        public string EMail { get; set; }

        [JsonProperty("min_sum")]
        public double MinSum { get; set; }

        [JsonProperty("tariff")]
        public double Tariff { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        
        [JsonProperty("closest_distance")]
        public double ClosestDistance { get; set; }

        [JsonProperty("closest_duration")]
        public string ClosestDuration { get; set; }

        [JsonProperty("closest_worker_id")]
        public int ClosestWorkerId { get; set; }
    }
}
