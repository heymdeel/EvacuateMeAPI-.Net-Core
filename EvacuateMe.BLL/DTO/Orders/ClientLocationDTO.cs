using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace EvacuateMe.BLL.DTO
{
    public class ClientLocationDTO
    {
        [Required, Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
        public double Latitude { get; set; }

        [Required, Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
        public double Longitude { get; set; }

        [Required]
        [JsonProperty("car_type")]
        public int CarType { get; set; }
    }
}
