using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvacuateMe.DAL.Entities
{
    [Table("sms_codes", Schema = "public"),]
    public class SMSCode
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("code")]
        public int Code { get; set; }

        [Column("time_stamp")]
        public DateTime TimeStamp { get; set; }
    }
}
