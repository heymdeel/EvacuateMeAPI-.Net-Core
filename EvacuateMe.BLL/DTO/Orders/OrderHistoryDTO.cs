using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.DTO.Orders
{
    public class OrderHistoryDTO
    {
        [JsonProperty("order_id")]
        public int Id { get; set; }

        [JsonProperty("beginning_time")]
        public DateTime BeginingTime { get; set; }

        [JsonProperty("termination_time")]
        public DateTime TerminationTime { get; set; }

        [JsonProperty("car_type")]
        public string CarTypeName{ get; set; }

        [JsonProperty("summary")]
        public double Summary { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("rate")]
        public int Rate { get; set; }
    }
}
