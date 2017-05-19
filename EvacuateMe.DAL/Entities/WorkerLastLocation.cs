using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers_last_location", Schema = "public")]
    public class WorkerLastLocation
    {   
        [Column("worker")]
        [Key, ForeignKey("Worker")]
        public int WorkerId { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
