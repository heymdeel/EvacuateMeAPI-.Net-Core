using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.DTO
{
    public class ClientSignUpDTO
    {
        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 15 символов")]
        public string Name { get; set; }

        [Required, RegularExpression("^[7-8][0-9]{10}$")]
        public string Phone { get; set; }

        [Required, Range(1000, 9999, ErrorMessage = "Код должен принимать значения от 1000 до 9999")]
        public int Code { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина модели машины должна быть от 5 до 50 символов")]
        [JsonProperty("car_model")]
        public string CarModel { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина цвета машины должна быть от 3 до 50 символов")]
        [JsonProperty("car_colour")]
        public string CarColour { get; set; }
    }
}
