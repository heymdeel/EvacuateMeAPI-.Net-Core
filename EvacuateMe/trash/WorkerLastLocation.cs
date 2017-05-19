//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("workers_last_location", Schema = "public")]
//    public class WorkerLastLocation : Location
//    {
//        [Column("worker")]
//        [Key, ForeignKey("Worker")]
//        public int WorkerId { get; set; }

//        public virtual Worker Worker { get; set; }
//    }
//}
