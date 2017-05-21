using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.DTO.Orders
{
    public class OrderCreateDTO
    {
        [Required, Range(0, int.MaxValue)]
        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [Required, Range(0, int.MaxValue)]
        [JsonProperty("worker_id")]
        public int WorkerId { get; set; }

        [Required, Range(0, int.MaxValue)]
        [JsonProperty("car_type")]
        public int CarTypeId { get; set; }

        [Required, Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
        [JsonProperty("latitude")]
        public double StartClientLat { get; set; }

        [Required, Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
        [JsonProperty("longitude")]
        public double StartClientLong { get; set; }

        [Required]
        [JsonProperty("car_model")]
        public string CarModel { get; set; }

        [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Длина цвета машины должна быть от 3 до 50 символов")]
        [JsonProperty("car_colour")]
        public string CarColour { get; set; }
    }
}
