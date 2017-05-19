using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers_location_history", Schema = "public")]
    public class WorkerLocationHistory
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("time_stamp")]
        public DateTime TimeStamp { get; set; }

        [ForeignKey("Worker")]
        [Column("worker")]
        public int WorkerId { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
