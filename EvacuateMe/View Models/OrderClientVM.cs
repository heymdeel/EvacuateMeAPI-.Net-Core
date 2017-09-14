using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.ViewModels
{
    public class OrderClientVM
    {
        [JsonProperty("order_id")]
        public int Id { get; set; }

        [JsonProperty("latitude")]
        public double StartClientLat { get; set; }

        [JsonProperty("longitude")]
        public double StartClientLong { get; set; }
        
        [JsonProperty("car_model")]
        public string CarModel { get; set; }

        [JsonProperty("car_colour")]
        public string CarColour { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
        
        [JsonProperty("phone")]
        public string ClientPhone { get; set; }
    }
}
