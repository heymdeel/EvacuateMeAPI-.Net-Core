//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("workers_location_history", Schema = "public")]
//    public class WorkerLocationHistory : Location
//    {
//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("time_stamp")]
//        public DateTime TimeStamp { get; set; }

//        [ForeignKey("Worker")]
//        [Column("worker")]
//        public int WorkerId { get; set; }

//        public virtual Worker Worker { get; set; }
//    }
//}
