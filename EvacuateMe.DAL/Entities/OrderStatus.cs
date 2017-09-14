using LinqToDB.Mapping;
using System;
using System.Collections.Generic;


namespace EvacuateMe.DAL.Entities
{
    [Table("orders_status", Schema = "public")]
    public class OrderStatus : Entity
    {        
        [Column("description")]
        public string Description { get; set; }

        [Association(ThisKey = "Id", OtherKey = "OrderStatusId")]
        public IEnumerable<Order> Orders { get; set; }
    }
}
