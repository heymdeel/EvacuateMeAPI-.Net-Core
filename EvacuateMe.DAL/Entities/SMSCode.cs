using LinqToDB.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EvacuateMe.DAL.Entities
{
    [Table("sms_codes", Schema = "public"),]
    public class SMSCode : Entity
    {        
        [Column("phone")]
        public string Phone { get; set; }

        [Column("code")]
        public int Code { get; set; }

        [Column("time_stamp")]
        public DateTime TimeStamp { get; set; }
    }
}
