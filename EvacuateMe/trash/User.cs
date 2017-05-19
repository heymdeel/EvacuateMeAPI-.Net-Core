//using EvacuateMe.Services;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    public abstract class SystemUser
//    {
//        [Column("name")]
//        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 15 символов")]
//        public string Name { get; set; }

//        [Column("phone")]
//        [Required, RegularExpression("^[7-8][0-9]{10}$")]
//        public string Phone { get; set; }

//        [Column("api_key")]
//        public string ApiKey { get; set; }

//        public virtual ICollection<Order> Orders { get; set; }

//        public static bool ValidatePhone(string phone)
//        {
//            if (!Regex.IsMatch(phone, "^[7-8][0-9]{10}$"))
//            {
//                return false;
//            }

//            return true;
//        }

//        //public abstract Task<bool> SignUpAsync(int code, DataContext db, IEncrypt encrypt);

//        //public bool SignIn(int code, DataContext db, IEncrypt encrypt)
//        //{
//        //    var sms = db.SMSCodes.FirstOrDefault(s => s.Phone == Phone && s.Code == code);
//        //    if (sms == null)
//        //    {
//        //        return false;
//        //    }

//        //    db.SMSCodes.Remove(sms);
//        //    ApiKey = encrypt.GenerateHash(Phone, code.ToString());

//        //    db.SaveChanges();
//        //    return true;
//        //}
//    }
//}
