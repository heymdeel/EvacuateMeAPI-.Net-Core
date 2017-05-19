//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace EvacuateMe.Models
//{
//    public enum OrderStatusEnum
//    {
//        Awaiting = 0,
//        OnTheWay = 1,
//        Performing = 2,
//        Completed = 3,
//        CanceledByWorker = 4,
//        CanceledByClient = 5
//    }

//    [Table("orders_status", Schema = "public")]
//    public class OrderStatus
//    {
//        [Key, Column("id")]
//        public int Id { get; set; }

//        [Column("description")]
//        [Required, StringLength(20, MinimumLength = 3, ErrorMessage = "Длина статуса заказа должна быть от 3 до 20 символов")]
//        public string Description { get; set; }

//        public virtual ICollection<Order> Orders { get; set; }
//    }
//}
