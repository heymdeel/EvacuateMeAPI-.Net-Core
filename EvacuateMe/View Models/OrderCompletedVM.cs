using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.ViewModels
{
    public class CompletedOrderVM
    {
        [JsonProperty("order_id")]
        public int Id { get; set; }

        [JsonProperty("summary")]
        public double Summary { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }
}
