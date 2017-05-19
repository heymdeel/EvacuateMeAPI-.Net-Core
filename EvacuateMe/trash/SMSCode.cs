//using EvacuateMe.Services;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("sms_codes", Schema = "public"),]
//    public class SMSCode
//    {
//        [Key]
//        [Column("id")]
//        public int Id { get; set; }

//        [Column("phone")]
//        [Required, RegularExpression("^[7-8][0-9]{10}$")]
//        public string Phone { get; set; }

//        [Column("code")]
//        [Required, Range(1000, 9999, ErrorMessage = "Код должен принимать значения от 1000 до 9999")]
//        public int Code { get; set; }

//        [Column("time_stamp")]
//        public DateTime TimeStamp { get; set; }

//        public static async Task GenerateAsync(string phone, int code, DataContext db)
//        {
//            var sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == phone);

//            if (sms == null)
//                db.SMSCodes.Add(new SMSCode() { Phone = phone, Code = code, TimeStamp = DateTime.Now });
//            else
//            {
//                sms.Code = code;
//                sms.TimeStamp = DateTime.Now;
//                db.SMSCodes.Update(sms);
//            }

//            await db.SaveChangesAsync();
//        }
//    }
//}
