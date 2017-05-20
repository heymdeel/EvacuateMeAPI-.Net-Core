using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.DAL.Entities;
using EvacuateMe.DAL.Interfaces;
using System.Linq;
using System.Text.RegularExpressions;
using EvacuateMe.BLL.DTO;
using AutoMapper;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork db;
        private readonly IEncrypt encryptService;
        private readonly IMapService mapService;

        public WorkerService(IUnitOfWork db, IEncrypt encrypt, IMapService mapService)
        {
            this.db = db;
            this.encryptService = encrypt;
            this.mapService = mapService;
        }

        public bool WorkerExists(string phone)
        {
            var matchingWorkers = db.Workers.Get(c => c.Phone == phone);
            return (matchingWorkers.Count() != 0);
        }

        public bool ValidatePhone(string phone)
        {
            return Regex.IsMatch(phone, "^[7-8][0-9]{10}$");
        }

        public Worker SignIn(SmsInfo smsInfo)
        {
            var sms = db.SMSCodes.FirstOrDefault(s => s.Phone == smsInfo.Phone && s.Code == smsInfo.Code);
            var worker = db.Workers.FirstOrDefault(c => c.Phone == smsInfo.Phone);
            if (sms == null || worker == null)
            {
                return null;
            }

            db.SMSCodes.Remove(sms);
            worker.ApiKey = encryptService.GenerateHash(sms.Phone, sms.Code.ToString());
            db.Workers.Update(worker);

            return worker;
        }

        public Worker GetByApiKey(string apiKey)
        {
            return db.Workers.FirstOrDefault(c => c.ApiKey == apiKey);
        }

        public bool ChangeStatus(Worker worker, int newStatus)
        {
            if (!Enum.IsDefined(typeof(WorkerStatusEnum), newStatus))
            {
                return false;
            }

            if ((worker.StatusId == (int)WorkerStatusEnum.Offline && newStatus == (int)WorkerStatusEnum.Working)
                || (worker.StatusId == (int)WorkerStatusEnum.Working && newStatus == (int)WorkerStatusEnum.Offline)
                || (worker.StatusId == (int)WorkerStatusEnum.Working && newStatus == (int)WorkerStatusEnum.PerformingOrder)
                || (worker.StatusId == (int)WorkerStatusEnum.PerformingOrder && newStatus == (int)WorkerStatusEnum.Offline))
            {
                worker.StatusId = newStatus;
                db.Workers.Update(worker);

                return true;
            }

            return false;
        }

        public bool ChangeLocation(Worker worker, Location newLocation)
        {
            if (worker.StatusId == (int)WorkerStatusEnum.Offline)
            {
                return false;
            }

            db.WorkersLocationHistory.Create(new WorkerLocationHistory()
            {
                Latitude = newLocation.Latitude,
                Longitude = newLocation.Longitude,
                TimeStamp = DateTime.Now,
                WorkerId = worker.Id
            });

            var lastLocation = db.WorkersLastLocation.FindById(worker.Id);
            if (lastLocation == null)
            {
                db.WorkersLastLocation.Create(new WorkerLastLocation()
                {
                    Latitude = newLocation.Latitude,
                    Longitude = newLocation.Longitude,
                    Worker = worker
                });
            }
            else
            {
                lastLocation.Latitude = newLocation.Latitude;
                lastLocation.Longitude = newLocation.Longitude;
                db.WorkersLastLocation.Update(lastLocation);
            }

            return true;
        }

        public IEnumerable<LocationHistoryDTO> GetLocationHistory(Worker worker)
        {
            return null;
        }

        public async Task<OrderInfoDTO> CheckForOrdersAsync(Worker worker)
        {
            var orders = db.Orders.GetWithInclude(o => o.WorkerId == worker.Id
                        && o.StatusId == (int)OrderStatusEnum.Awaiting,
                        c => c.Client);

            if (orders.Count() == 0)
            {
                return null;
            }

            var order = orders.First();
            var orderInfo = Mapper.Map<Order, OrderInfoDTO>(order);
            orderInfo.ClientPhone = order.Client.Phone;
            orderInfo.Distance = await mapService.GetDistanceAsync(order.StartClientLat, order.StartClientLong, 
                                                        order.StartWorkerLat, order.StartWorkerLong);

            return orderInfo;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
