using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers_last_location", Schema = "public")]
    public class WorkerLastLocation : Entity
    {   
        [Column("worker")]
        public override int Id { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        public Worker Worker { get; set; }
    }
}
