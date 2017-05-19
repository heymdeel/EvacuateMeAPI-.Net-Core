using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.BuisnessModels
{
    public class Car
    {
        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Длина модели машины должна быть от 5 до 50 символов")]
        [JsonProperty("car_model")]
        public string Model { get; set; }

        [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Длина цвета машины должна быть от 3 до 50 символов")]
        [JsonProperty("car_colour")]
        public string Colour { get; set; }
    }
}
