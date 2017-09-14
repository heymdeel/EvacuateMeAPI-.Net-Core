using LinqToDB.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EvacuateMe.DAL.Entities
{
    [Table("workers", Schema = "public")]
    public class Worker : Entity
    {        
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

        [Column("company")]
        public int CompanyId { get; set; }

        [Column("status")]
        public int StatusId { get; set; }

        [Column("supported_car_type")]
        public int CarTypeId { get; set; }

        public Company Company { get; set; }

        public CarType CarType { get; set; }

        [Association(ThisKey = "Id", OtherKey = "WorkerId")]
        public IEnumerable<WorkerLocationHistory> LocationHistory { get; set; }

        [Association(ThisKey = "Id", OtherKey = "Id")]
        public WorkerLastLocation LastLocation { get; set; }

        [Association(ThisKey = "Id", OtherKey = "WorkerId")]
        public IEnumerable<Order> Order { get; set; }
    }
}

