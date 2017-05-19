using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EvacuateMe.BLL.BuisnessModels
{
    public class Location
    {
        [Required, Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
        public double Latitude { get; set; }

        [Required, Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
        public double Longitude { get; set; }
    }
}
