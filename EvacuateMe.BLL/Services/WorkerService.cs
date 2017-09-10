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
using EvacuateMe.BLL.DTO.Workers;

namespace EvacuateMe.BLL.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork db;
        private readonly IEncrypt encryptService;
        private readonly IMapService mapService;

        public WorkerService(IUnitOfWork db, IEncrypt encryptService, IMapService mapService)
        {
            this.db = db;
            this.encryptService = encryptService;
            this.mapService = mapService;
        }

        public async Task<bool> WorkerExistsAsync(string phone)
        {
            Worker worker = await db.Workers.FirstOrDefaultAsync(c => c.Phone == phone);
            return (worker != null);
        }

        public bool ValidatePhone(string phone)
        {
            return Regex.IsMatch(phone, "^[7-8][0-9]{10}$");
        }

        public async Task<Worker> SignInAsync(SmsDTO smsInfo)
        {
            SMSCode sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == smsInfo.Phone && s.Code == smsInfo.Code);
            Worker worker = await db.Workers.FirstOrDefaultAsync(c => c.Phone == smsInfo.Phone);
            if (sms == null || worker == null)
            {
                return null;
            }

            await db.SMSCodes.RemoveAsync(sms);
            worker.ApiKey = encryptService.GenerateHash(sms.Phone, sms.Code.ToString());
            await db.Workers.UpdateAsync(worker);

            return worker;
        }

        public async Task<Worker> GetByApiKeyAsync(string apiKey)
        {
            return await db.Workers.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
        }

        public async Task<bool> ChangeStatusAsync(Worker worker, int newStatus)
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
                await db.Workers.UpdateAsync(worker);

                return true;
            }

            return false;
        }

        public async Task<bool> ChangeLocationAsync(Worker worker, LocationDTO newLocation)
        {
            if (worker.StatusId == (int)WorkerStatusEnum.Offline)
            {
                return false;
            }

            var locationHistory = Mapper.Map<LocationDTO, WorkerLocationHistory>(newLocation);
            locationHistory.TimeStamp = DateTime.Now;
            locationHistory.WorkerId = worker.Id;

            await db.WorkersLocationHistory.CreateAsync(locationHistory);

            var lastLocation = await db.WorkersLastLocation.FindByIdAsync(worker.Id);
            if (lastLocation == null)
            {
                await db.WorkersLastLocation.CreateAsync(new WorkerLastLocation()
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
                await db.WorkersLastLocation.UpdateAsync(lastLocation);
            }

            return true;
        }

        public async Task<OrderClientDTO> CheckForOrdersAsync(Worker worker)
        {
            var order = await db.Orders.FirstOrDefaultAsync(filter: o => o.WorkerId == worker.Id
                        && o.StatusId == (int)OrderStatusEnum.Awaiting,
                        include: c => c.Client);

            if (order == null)
            {
                return null;
            }

            var orderInfo = Mapper.Map<Order, OrderClientDTO>(order);
            orderInfo.ClientPhone = order.Client.Phone;
            orderInfo.Distance = await mapService.GetDistanceAsync(order.StartClientLat, order.StartClientLong, order.StartWorkerLat, order.StartWorkerLong);

            return orderInfo;
        }

        public async Task SignUpAsync(WorkerSignUpDTO workerInfo, int companyId)
        {
            var worker = Mapper.Map<WorkerSignUpDTO, Worker>(workerInfo);

            worker.ApiKey = encryptService.GenerateHash(worker.Phone, "key");
            worker.CarTypeId = 1;
            worker.CompanyId = companyId;
            worker.DateOfHire = DateTime.Now;
            worker.StatusId = 0;

            await db.Workers.CreateAsync(worker);
        }
    }
}
