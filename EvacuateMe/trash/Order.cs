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
//    [Table("orders", Schema = "public")]
//    public class Order
//    {
//        private static Dictionary<OrderStatusEnum, List<OrderStatusEnum>> workerPermissions = new Dictionary<OrderStatusEnum, List<OrderStatusEnum>>()
//            {
//                {OrderStatusEnum.Awaiting, new List<OrderStatusEnum>() { OrderStatusEnum.OnTheWay, OrderStatusEnum.CanceledByWorker }},
//                {OrderStatusEnum.OnTheWay, new List<OrderStatusEnum>() { OrderStatusEnum.Performing, OrderStatusEnum.CanceledByWorker } },
//                {OrderStatusEnum.Performing, new List<OrderStatusEnum>() { OrderStatusEnum.Completed } }
//            };

//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("client"), ForeignKey("Client")]
//        public int ClientId { get; set; }

//        [Column("worker"), ForeignKey("Worker")]
//        [Required]
//        [JsonProperty("worker_id")]
//        public int WorkerId { get; set; }

//        [Column("start_client_lat")]
//        [Required, Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
//        [JsonProperty("latitude")]
//        public double StartClientLat { get; set; }

//        [Column("start_client_long")]
//        [Required, Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
//        [JsonProperty("longitude")]
//        public double StartClientLong { get; set; }

//        [Column("start_worker_lat")]
//        [Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
//        public double StartWorkerLat { get; set; }

//        [Column("start_worker_long")]
//        [Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
//        public double StartWorkerLong { get; set; }

//        [Column("beginning_time")]
//        public DateTime BeginingTime { get; set; }

//        [Column("termination_time")]
//        public DateTime TerminationTime { get; set; }

//        [Column("final_lat")]
//        [Range(-180d, 180d, ErrorMessage = "Значение широты должно быть от -180 до 180")]
//        public double FinalLat { get; set; }

//        [Column("final_long")]
//        [Range(-180d, 180d, ErrorMessage = "Значение долготы должно быть от -180 до 180")]
//        public double FinalLong { get; set; }

//        [Column("car_model")]
//        [Required, StringLength(50, MinimumLength = 5, ErrorMessage = "Длина модели машины должна быть от 5 до 50 символов")]
//        [JsonProperty("car_model")]
//        public string CarModel { get; set; }

//        [Column("car_colour")]
//        [Required, StringLength(50, MinimumLength = 3, ErrorMessage = "Длина цвета машины должна быть от 3 до 50 символов")]
//        [JsonProperty("car_colour")]
//        public string CarColour { get; set; }

//        [Column("car_type"), ForeignKey("CarType")]
//        [Required]
//        [JsonProperty("car_type")]
//        public int CarTypeId { get; set; }

//        [Column("summary")]
//        public double Summary { get; set; }

//        [Column("distance")]
//        public double Distance { get; set; }

//        [Column("rate")]
//        public int Rate { get; set; }

//        [Column("status"), ForeignKey("Status")]
//        public int StatusId { get; set; }

//        public virtual Client Client { get; set; }

//        public virtual Worker Worker { get; set; }

//        public virtual CarType CarType { get; set; }

//        public virtual OrderStatus Status { get; set; }

//        private bool StatusIsCorrect(int oldStatus, int newStatus)
//        {
//            if (!Enum.IsDefined(typeof(OrderStatusEnum), newStatus))
//            {
//                return false;
//            }

//            if (!workerPermissions.ContainsKey((OrderStatusEnum)oldStatus))
//            {
//                return false;
//            }

//            if (workerPermissions[(OrderStatusEnum)oldStatus].Contains((OrderStatusEnum)newStatus))
//            {
//                return true;
//            }

//            return false;
//        }

//        public bool ChangeStatus(int newStatus, SystemUser user, IMapService map, DataContext db)
//        {
//            if (user is Client)
//            {
//                db.Entry(this).Reference(o => o.Client).Load();
//                if ((user as Client).Id != this.Client.Id)
//                {
//                    return false;
//                }

//                if ((StatusId == (int)OrderStatusEnum.Awaiting || StatusId == (int)OrderStatusEnum.OnTheWay)
//                    && newStatus == (int)OrderStatusEnum.CanceledByClient)
//                {
//                    this.StatusId = newStatus;
//                    db.Entry(this).Reference(o => o.Worker).Load();
//                    this.Worker.StatusId = (int)WorkerStatus.Offline;

//                    db.SaveChanges();
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
           
//            if (user is Worker)
//            {
//                db.Entry(this).Reference(o => o.Worker).Load();
//                if ((user as Worker).Id != this.Worker.Id)
//                {
//                    return false;
//                }

//                if (StatusIsCorrect(StatusId, newStatus))
//                {
//                    this.StatusId = newStatus;
//                    db.Entry(this).Reference(o => o.Worker).Load();
//                    db.Entry(Worker).Reference(w => w.Company).Load();

//                    if (newStatus == (int)OrderStatusEnum.OnTheWay || newStatus == (int)OrderStatusEnum.Performing)
//                    {
//                        this.Worker.StatusId = (int)WorkerStatus.PerformingOrder;
//                    }
                    
//                    if (newStatus == (int)OrderStatusEnum.CanceledByWorker)
//                    {
                     
//                        this.Worker.StatusId = (int)WorkerStatus.Offline;
    
//                        Worker.Company.CountRate += 1;
//                        Worker.Company.SumRate += 1;
//                        this.Rate = 1;
//                    }

//                    if (newStatus == (int)OrderStatusEnum.Completed)
//                    {
//                        this.Worker.StatusId = (int)WorkerStatus.Offline;
//                        var workerLocation = Worker.GetCurrentLocation(db);
//                        this.FinalLat = workerLocation.Latitude;
//                        this.FinalLong = workerLocation.Longitude;
//                        this.TerminationTime = DateTime.Now;

//                        Distance = Task.Run(async () => await map.GetDistanceAsync(StartClientLat, StartClientLong, FinalLat, FinalLong)).Result;
//                        Summary = (Distance * Worker.Company.Tariff) / 1000d + Worker.Company.MinSum;

//                    }

//                    db.SaveChanges();
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            return false;
//        }

//        public bool CheckUser(SystemUser user, DataContext db)
//        {
//            if (user == null)
//            {
//                return false;
//            }

//            if (StatusId != (int)OrderStatusEnum.Completed)
//            {
//                return false;
//            }

//            if (user is Client)
//            {
//                db.Entry(this).Reference(o => o.Client).Load();
//                if (Client.Id != ((Client)user).Id)
//                {
//                    return false;
//                }
//            }

//            if (user is Worker)
//            {
//                db.Entry(this).Reference(o => o.Worker).Load();
//                if (Worker.Id != ((Worker)user).Id)
//                {
//                    return false;
//                }
//            }

//            return true;
//        }
//    }
//}
