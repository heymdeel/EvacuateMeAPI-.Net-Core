using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.ViewModels
{
    public class OrderWorkerVM
    {
        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
