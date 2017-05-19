//using EvacuateMe.Services;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    [Table("companies", Schema = "public")]
//    public class Company
//    {  
//        [Key, Column("id")]
//        [JsonProperty("id")]
//        public int Id { get; set; }

//        [Column("name")]
//        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 20 символов")]
//        [JsonProperty("name")]
//        public string Name { get; set; }
        
//        [Column("description")]
//        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 20 символов")]
//        [JsonProperty("description")]
//        public string Description { get; set; }

//        [Column("address")]
//        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 20 символов")]
//        [JsonProperty("address")]
//        public string Address { get; set; }

//        [Column("contact_phone")]
//        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 20 символов")]
//        [JsonProperty("contact_phone")]
//        public string ContactPhone { get; set; }

//        [Column("email")]
//        [Required, StringLength(25, MinimumLength = 3, ErrorMessage = "Длина имени должна быть от 3 до 25 символов")]
//        [JsonProperty("email")]
//        public string EMail { get; set; }
        
//        [Column("min_sum")]
//        [Required, Range(100d, 10000d)]
//        [JsonProperty("min_sum")]
//        public double MinSum { get; set; }
        
//        [Column("tariff")]
//        [Required, Range(100d, 10000d)]
//        [JsonProperty("tariff")]
//        public double Tariff { get; set; }

//        [Column("sum_rate")]
//        [Range(0, int.MaxValue)]
//        [JsonIgnore]
//        public int SumRate { get; set; }

//        [Column("count_rate")]
//        [Range(0, int.MaxValue)]
//        [JsonIgnore]
//        public int CountRate { get; set; }

//        [Column("logo_url")]
//        [Required]
//        [JsonProperty("logo_url")]
//        public string LogoUrl { get; set; }

//        [Column("login")]
//        [Required, StringLength(15, MinimumLength = 3, ErrorMessage = "Длина логина должна быть от 3 до 15 символов")]
//        [JsonIgnore]
//        public string Login { get; set; }
    
//        [Column("password")]
//        [Required]
//        [JsonIgnore]
//        public string Password { get; set; }

//        [Column("api_key")]
//        [JsonIgnore]
//        public string ApiKey { get; set; }

//        [JsonIgnore]
//        public virtual ICollection<Worker> Workers { get; set; }

//        public async Task<CompanyJsonInfo> GetClosestWorkerAsync(int carType, Location clientLocation, IMapService map, DataContext db)
//        {
//            double minDistance = -1;
//            string minDuration = null;
//            int closestWorkerId = -1;

//            db.Entry(this).Collection(c => c.Workers).Load();
//            foreach (var worker in Workers)
//            {
//                if (worker.StatusId != (int)WorkerStatus.Working || worker.CarTypeId != carType)
//                    continue;

//                var workerLocation = await db.WorkerLastLocation.FindAsync(worker.Id);

//                var distance = await map.GetDistanceAsync(clientLocation.Latitude, clientLocation.Longitude, workerLocation.Latitude, workerLocation.Longitude);
//                var duration = await map.GetDurationAsync(clientLocation.Latitude, clientLocation.Longitude, workerLocation.Latitude, workerLocation.Longitude);

//                if (minDistance == -1 || distance < minDistance)
//                {
//                    minDistance = distance;
//                    minDuration = duration;
//                    closestWorkerId = worker.Id;
//                }
//            }

//            if (minDistance != -1)
//            {
//                return new CompanyJsonInfo()
//                {
//                    Id = Id,
//                    Name = Name,
//                    Description = Description,
//                    MinSum = MinSum,
//                    Address = Address,
//                    ContactPhone = ContactPhone,
//                    EMail = EMail,
//                    LogoUrl = LogoUrl,
//                    Rate = CountRate == 0 ? 0 : (double)SumRate / (double)CountRate,
//                    Tariff = Tariff,
//                    ClosestDistance = minDistance,
//                    ClosestDuration = minDuration,
//                    ClosestWorkerId = closestWorkerId
//                };
//            }

//            return null;
//        }
//    }
//}
