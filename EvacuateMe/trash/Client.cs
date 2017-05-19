//using EvacuateMe.Services;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("clients", Schema = "public"),]
//    public class Client : SystemUser
//    {
//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("car_model")]
//        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина модели машины должна быть от 5 до 50 символов")]
//        [JsonProperty("car_model")]
//        public string CarModel { get; set; }

//        [Column("car_colour")]
//        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина цвета машины должна быть от 3 до 50 символов")]
//        [JsonProperty("car_colour")]
//        public string CarColour { get; set; }

//        public static bool Exists(string phone, DataContext db)
//        {
//            if (db.Clients.Any(c => c.Phone == phone))
//            {
//                return true;
//            }

//            return false;
//        }

//        //public override async Task<bool> SignUpAsync(int code, DataContext db, IEncrypt encrypt)
//        //{
//        //    var sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == Phone && s.Code == code);
//        //    if (sms == null)
//        //    {
//        //        return false;
//        //    }

//        //    db.SMSCodes.Remove(sms);
//        //    ApiKey = encrypt.GenerateHash(Phone, code.ToString());
//        //    db.Clients.Add(this);
//        //    await db.SaveChangesAsync();

//        //    return true;
//        //}

//        public async Task<bool> MakeOrderAsync(Company company, Worker worker, Order order, DataContext db)
//        {
//            if (company == null || worker == null)
//            {
//                return false;
//            }

//            db.Entry(company).Collection(c => c.Workers).Load();
//            if (!company.Workers.Contains(worker))
//            {
//                return false;
//            }

//            if (worker.StatusId != (int)WorkerStatus.Working)
//            {
//                return false;
//            }

//            var workerLocation = worker.GetCurrentLocation(db);
//            if (workerLocation == null)
//            {
//                return false;
//            }

//            order.ClientId = this.Id;
//            order.StartWorkerLat = workerLocation.Latitude;
//            order.StartWorkerLong = workerLocation.Longitude;
//            order.BeginingTime = DateTime.Now;
//            order.StatusId = (int)OrderStatusEnum.Awaiting;
//            order.Rate = 0;
//            db.Orders.Add(order);

//            await db.SaveChangesAsync();

//            return true;
//        }

//        private bool ValidateRate(Order order, int rate, DataContext db)
//        {
//            if (rate < 1 || rate > 5)
//            {
//                return false;
//            }

//            db.Entry(order).Reference(o => o.Client).Load();
//            if (order.Client.Id != this.Id)
//            {
//                return false;
//            }

//            if (order.Rate != 0)
//            {
//                return false;
//            }

//            if (order.StatusId != (int)OrderStatusEnum.Completed ||
//                (order.TerminationTime.AddMinutes(15) < DateTime.Now))
//            {
//                return false;
//            }

//            return true;
//        }
//        public bool RateOrder(Order order, int  rate, DataContext db)
//        {
//            if (!ValidateRate(order, rate, db))
//            {
//                return false;
//            }

//            db.Entry(order).Reference(o => o.Worker).Load();
//            db.Entry(order.Worker).Reference(w => w.Company).Load();
//            Console.WriteLine("asdasda " + order.Worker.Company.Name);
//            order.Worker.Company.SumRate += rate;
//            order.Worker.Company.CountRate += 1;
//            order.Rate = rate;

//            db.SaveChanges();

//            return true;
//        }
//    }
//}
