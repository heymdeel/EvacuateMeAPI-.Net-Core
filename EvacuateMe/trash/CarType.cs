//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("car_type", Schema = "public")]
//    public class CarType
//    {
//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("name")]
//        [Required, StringLength(30, MinimumLength = 5, ErrorMessage = "Длина типа машины должна быть от 5 до 30 символов")]
//        public string Name { get; set; }

//        public virtual ICollection<Order> Orders { get; set; }

//        public virtual ICollection<Worker> Workers { get; set; }
//    }
//}
