//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    public class Location
//    {
//        [Column("latitude")]
//        [Required, Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
//        public double Latitude { get; set; }

//        [Column("longitude")]
//        [Required, Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
//        public double Longitude { get; set; }
//    }
//}
