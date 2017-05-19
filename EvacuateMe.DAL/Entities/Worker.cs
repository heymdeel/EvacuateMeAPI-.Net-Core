using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers", Schema = "public"),]
    public class Worker
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("api_key")]
        public string ApiKey { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("patronymic")]
        public string Patronymic { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("date_of_hire")]
        public DateTime DateOfHire { get; set; }

        [Column("car_number")]
        public string CarNumber { get; set; }

        [ForeignKey("Company")]
        [Column("company")]
        public int CompanyId { get; set; }

        [ForeignKey("Status")]
        [Column("status")]
        public int StatusId { get; set; }

        [ForeignKey("CarType")]
        [Column("supported_car_type")]
        public int CarTypeId { get; set; }

        public virtual Company Company { get; set; }

        public virtual CarType CarType { get; set; }

        public virtual ICollection<WorkerLocationHistory> LocationHistory { get; set; }

        public virtual WorkerLastLocation LastLocation { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}

