using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("orders_status", Schema = "public")]
    public class OrderStatus : Entity
    {        
        [Column("description")]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
