using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("clients", Schema = "public"),]
    public class Client : Entity
    {        
        [Column("name")]
        public string Name { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("api_key")]
        public string ApiKey { get; set; }

        [Column("car_model")]
        public string CarModel { get; set; }

        [Column("car_colour")]
        public string CarColour { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
