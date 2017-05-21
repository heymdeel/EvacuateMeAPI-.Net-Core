using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.DTO.Orders
{
    public class CompletedOrderDTO
    {
        [JsonProperty("order_id")]
        public int Id { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("summary")]
        public double Summary { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}
