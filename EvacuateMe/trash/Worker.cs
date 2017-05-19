//using EvacuateMe.Services;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("workers", Schema = "public"),]
//    public class Worker : SystemUser
//    {
//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("surname")]
//        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина фамилии должна быть от 3 до 15 символов")]
//        public string Surname { get; set; }

//        [Column("patronymic")]
//        [StringLength(15, MinimumLength = 3, ErrorMessage = "Длина отчества должна быть от 3 до 15 символов")]
//        public string Patronymic { get; set; }

//        [Column("date_of_birth")]
//        [Required]
//        [JsonProperty("date_of_birth")]
//        public DateTime DateOfBirth { get; set; }

//        [Column("date_of_hire")]
//        public DateTime DateOfHire { get; set; }

//        [Column("car_number")]
//        [Required, StringLength(15, MinimumLength = 5, ErrorMessage = "Длина номера машины должна быть от 5 до 15 символов")]
//        [JsonProperty("car_number")]
//        public string CarNumber { get; set; }

//        [ForeignKey("Company")]
//        [Column("company")]
//        public int CompanyId { get; set; }

//        [ForeignKey("Status")]
//        [Column("status")]
//        public int StatusId { get; set; }

//        [ForeignKey("CarType")]
//        [Column("supported_car_type")]
//        public int CarTypeId { get; set; }

//        public virtual Company Company { get; set; }

//        public virtual CarType CarType { get; set; }
        
//        //public virtual ICollection<WorkerLocationHistory> LocationHistory { get; set; }

//        //public virtual WorkerLastLocation LastLocation { get; set; }

//        public static bool Exists(string phone, DataContext db)
//        {
//            if (db.Workers.Any(c => c.Phone == phone))
//            {
//                return true;
//            }

//            return false;
//        }

//        public bool IsOffline()
//        {
//            if (StatusId == (int)WorkerStatus.Offline)
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
//        //    db.Workers.Add(this);
//        //    await db.SaveChangesAsync();

//        //    return true;
//        //}

//        public async Task ChangeLocationAsync(Location newLocation, DataContext db)
//        {
//            db.WorkersLocationHistory.Add(new WorkerLocationHistory()
//            {
//                Latitude = newLocation.Latitude,
//                Longitude = newLocation.Longitude,
//                TimeStamp = DateTime.Now,
//                Worker = this
//            });

//            var lastLocation = await db.WorkerLastLocation.FindAsync(Id);
//            if (lastLocation == null)
//            {
//                db.WorkerLastLocation.Add(new WorkerLastLocation()
//                {
//                    Latitude = newLocation.Latitude,
//                    Longitude = newLocation.Longitude,
//                    Worker = this
//                });
//            }
//            else
//            {
//                lastLocation.Latitude = newLocation.Latitude;
//                lastLocation.Longitude = newLocation.Longitude;
//            }

//            await db.SaveChangesAsync();
//        }

//        public Location GetCurrentLocation(DataContext db)
//        {
//            db.Entry(this).Reference(w => w.LastLocation).Load();
            
//            if (LastLocation != null)
//            {
//                return new Location() { Latitude = LastLocation.Latitude, Longitude = LastLocation.Longitude };
//            }

//            return null;
//        }

//        public async Task<bool> ChangeStatus(int newStatus, DataContext db)
//        {
//            if (!Enum.IsDefined(typeof(WorkerStatus), newStatus))
//            {
//                return false;
//            }

//            if ((StatusId == (int)WorkerStatus.Offline && newStatus == (int)WorkerStatus.Working)
//                || (StatusId == (int)WorkerStatus.Working && newStatus == (int)WorkerStatus.Offline)
//                || (StatusId == (int)WorkerStatus.Working && newStatus == (int)WorkerStatus.PerformingOrder)
//                || (StatusId == (int)WorkerStatus.PerformingOrder && newStatus == (int)WorkerStatus.Offline))
//            {
//                StatusId = newStatus;
//                await db.SaveChangesAsync();

//                return true;
//            }

//            return false;
//        }
//    }
//}

