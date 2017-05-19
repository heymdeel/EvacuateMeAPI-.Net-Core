using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("orders_status", Schema = "public")]
    public class OrderStatus
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("description")]
        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
