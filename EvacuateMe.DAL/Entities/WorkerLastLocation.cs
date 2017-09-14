using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers_last_location", Schema = "public")]
    public class WorkerLastLocation : Entity
    {   
        [Column("worker")]
        [Key, ForeignKey("Worker")]
        public override int Id { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
