using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers_location_history", Schema = "public")]
    public class WorkerLocationHistory : Entity
    {        
        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("time_stamp")]
        public DateTime TimeStamp { get; set; }

        [Column("worker")]
        public int WorkerId { get; set; }

        public Worker Worker { get; set; }
    }
}
