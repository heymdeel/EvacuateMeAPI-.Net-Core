using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

namespace EvacuateMe.DAL.Entities
{
    [Table("car_type", Schema = "public")]
    public class CarType : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        [Association(ThisKey = "Id", OtherKey = "CarTypeId")]
        public IEnumerable<Order> Orders { get; set; }

        [Association(ThisKey = "Id", OtherKey = "CarTypeId")]
        public IEnumerable<Worker> Workers { get; set; }
    }
}
