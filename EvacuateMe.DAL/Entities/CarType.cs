using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("car_type", Schema = "public")]
    public class CarType : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Worker> Workers { get; set; }
    }
}
