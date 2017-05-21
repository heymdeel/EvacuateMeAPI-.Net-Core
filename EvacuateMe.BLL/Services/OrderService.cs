using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Interfaces;
using EvacuateMe.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EvacuateMe.BLL.DTO.Orders;
using EvacuateMe.BLL.BuisnessModels;
using MoreLinq;

namespace EvacuateMe.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork db;
        private readonly IMapService mapService;

        private static Dictionary<OrderStatusEnum, List<OrderStatusEnum>> workerPermissions = new Dictionary<OrderStatusEnum, List<OrderStatusEnum>>()
                    {
                        {OrderStatusEnum.Awaiting, new List<OrderStatusEnum>() { OrderStatusEnum.OnTheWay, OrderStatusEnum.CanceledByWorker }},
                        {OrderStatusEnum.OnTheWay, new List<OrderStatusEnum>() { OrderStatusEnum.Performing, OrderStatusEnum.CanceledByWorker } },
                        {OrderStatusEnum.Performing, new List<OrderStatusEnum>() { OrderStatusEnum.Completed } }
                    };

        public OrderService(IUnitOfWork db, IMapService mapService)
        {
            this.db = db;
            this.mapService = mapService;
        }

        public IEnumerable<OrderCompanyDTO> GetListOfCompanies(ClientLocationDTO clientInfo)
        {
            List<OrderCompanyDTO> result = new List<OrderCompanyDTO>();

            var companies = db.Companies.GetWithInclude(c => c.Workers.Any(w => w.StatusId == (int)WorkerStatus.Working), c => c.Workers);
            foreach (var company in companies)
            {
                var companyInfo = GetClosestWorker(company, clientInfo);
                if (companyInfo != null)
                {
                    result.Add(companyInfo);
                }
            }

            return result.Count == 0 ? null : result;
        }

        public OrderWorkerDTO CreateOrder(Client client, OrderCreateDTO orderInfo)
        {
            var company = db.Companies.FirstOrDefaultWithInclude(c => c.Id == orderInfo.CompanyId, c => c.Workers);
            var worker = db.Workers.FindById(orderInfo.WorkerId);

            if (!ValidateCompany(company, worker))
            {
                return null;
            }

            var workerLocation = db.WorkersLastLocation.FindById(worker.Id);
            if (workerLocation == null)
            {
                return null;
            }

            var order = Mapper.Map<OrderCreateDTO, Order>(orderInfo);

            order.ClientId = client.Id;
            order.StartWorkerLat = workerLocation.Latitude;
            order.StartWorkerLong = workerLocation.Longitude;
            order.BeginingTime = DateTime.Now;
            order.StatusId = (int)OrderStatusEnum.Awaiting;
            order.Rate = 0;
            db.Orders.Create(order);

            var response = new OrderWorkerDTO()
            {
                OrderId = order.Id,
                Name = worker.Name,
                Latitude = workerLocation.Latitude,
                Longitude = workerLocation.Longitude,
                Phone = worker.Phone
            };

            return response;
        }

        public LocationDTO GetWorkerLocation(int orderId)
        {
            var order = db.Orders.FindById(orderId);
            if (order == null)
            {
                return null;
            }

            var location = db.WorkersLastLocation.FindById(order.WorkerId);

            return Mapper.Map<WorkerLastLocation, LocationDTO>(location);
        }

        public bool ChangeStatusByClient(int orderId, int newStatus)
        {
            var order = db.Orders.FirstOrDefaultWithInclude(o => o.Id == orderId, o => o.Worker);

            if ((order.StatusId == (int)OrderStatusEnum.Awaiting || order.StatusId == (int)OrderStatusEnum.OnTheWay)
                && newStatus == (int)OrderStatusEnum.CanceledByClient)
            {
                order.StatusId = newStatus;
                order.Worker.StatusId = (int)WorkerStatus.Offline;

                db.Orders.Update(order);
                db.Workers.Update(order.Worker);

                return true;
            }

            return false;
        }

        public bool ChangeStatusByWorker(int orderId, int newStatus)
        {
            var order = db.Orders.FirstOrDefaultWithInclude(o => o.Id == orderId, o => o.Worker);

            if (StatusIsCorrect(order.StatusId, newStatus))
            {
                order.StatusId = newStatus;

                if (newStatus == (int)OrderStatusEnum.OnTheWay || newStatus == (int)OrderStatusEnum.Performing)
                {
                    order.Worker.StatusId = (int)WorkerStatus.PerformingOrder;
                }

                var company = db.Companies.FindById(order.Worker.CompanyId);

                if (newStatus == (int)OrderStatusEnum.CanceledByWorker)
                {
                    order.Worker.StatusId = (int)WorkerStatus.Offline;

                    company.CountRate += 1;
                    company.SumRate += 1;

                    order.Rate = 1;
                }

                if (newStatus == (int)OrderStatusEnum.Completed)
                {
                    order.Worker.StatusId = (int)WorkerStatus.Offline;

                    var workerLocation = db.WorkersLastLocation.FindById(order.WorkerId);
                    order.FinalLat = workerLocation.Latitude;
                    order.FinalLong = workerLocation.Longitude;
                    order.TerminationTime = DateTime.Now;

                    order.Distance = Task.Run(async ()
                        => await mapService.GetDistanceAsync(order.StartClientLat, order.StartClientLong,
                        order.FinalLat, order.FinalLong)).Result;
                    order.Summary = (order.Distance * company.Tariff) / 1000d + company.MinSum;

                }

                db.Orders.Update(order);
                db.Companies.Update(company);
                db.Workers.Update(order.Worker);
                return true;
            }

            return false;
        }

        public OrderStatus GetOrderStatus(int orderId)
        {
            var order = db.Orders.FirstOrDefaultWithInclude(o => o.Id == orderId, o => o.Status);

            return order?.Status;
        }

        public bool RateOrder(int orderId, int rate)
        {
            var order = db.Orders.FirstOrDefaultWithInclude(o => o.Id == orderId, o => o.Worker);
            if (order == null)
            {
                return false;
            }

            if (!ValidateRate(order, rate))
            {
                return false;
            }

            var company = db.Companies.FindById(order.Worker.CompanyId);

            company.SumRate += rate;
            company.CountRate += 1;
            order.Rate = rate;

            db.Orders.Update(order);
            db.Companies.Update(company);

            return true;
        }

        public CompletedOrderDTO GetOrderInfo(int orderId)
        {
            var order = db.Orders.FindById(orderId);

            if (order.StatusId != (int)OrderStatusEnum.Completed)
            {
                return null;
            }

            var orderInfo = Mapper.Map<Order, CompletedOrderDTO>(order);
            orderInfo.Company = db.Companies.FirstOrDefault(c => c.Workers.Any(w => w.Id == order.WorkerId)).Name;

            return orderInfo;
        }

        public IEnumerable<OrderHistoryDTO> GetClientHistory(Client client)
        {
            var orders = db.Orders.GetWithInclude(o => o.ClientId == client.Id, o => o.CarType);

            if (orders == null || orders.Count() == 0)
            {
                return null;
            }
            var history = new List<OrderHistoryDTO>();
            foreach (var order in orders)
            {
                var orderHistory = Mapper.Map<Order, OrderHistoryDTO>(order);
                orderHistory.CarTypeName = order.CarType.Name;
                history.Add(orderHistory);
            }

            return history;
        }

        public IEnumerable<OrderHistoryDTO> GetWorkerHistory(Worker worker)
        {
            var orders = db.Orders.GetWithInclude(o => o.WorkerId == worker.Id, o => o.CarType);

            if (orders == null || orders.Count() == 0)
            {
                return null;
            }
            var history = new List<OrderHistoryDTO>();
            foreach (var order in orders)
            {
                var orderHistory = Mapper.Map<Order, OrderHistoryDTO>(order);
                orderHistory.CarTypeName = order.CarType.Name;
                history.Add(orderHistory);
            }

            return history;
        }

        public bool ClientInOrder(int orderId, Client client)
        {
            var order = db.Orders.FindById(orderId);
            if (order?.ClientId == client.Id)
            {
                return true;
            }
           
            return false;
        }

        public bool WorkerInOrder(int orderId, Worker worker)
        {
            var order = db.Orders.FindById(orderId);
            if (order?.WorkerId == worker.Id)
            {
                return true;
            }

            return false;
        }

        private bool ValidateRate(Order order, int rate)
        {
            if (rate < 1 || rate > 5)
            {
                return false;
            }

            if (order.Rate != 0)
            {
                return false;
            }

            if (order.StatusId != (int)OrderStatusEnum.Completed ||
                (order.TerminationTime.AddDays(1) < DateTime.Now))
            {
                return false;
            }

            return true;
        }

        private bool StatusIsCorrect(int oldStatus, int newStatus)
        {
            if (!Enum.IsDefined(typeof(OrderStatusEnum), newStatus))
            {
                return false;
            }

            if (!workerPermissions.ContainsKey((OrderStatusEnum)oldStatus))
            {
                return false;
            }

            if (workerPermissions[(OrderStatusEnum)oldStatus].Contains((OrderStatusEnum)newStatus))
            {
                return true;
            }

            return false;
        }

        private bool ValidateCompany(Company company, Worker worker)
        {
            if (company == null || worker == null)
            {
                return false;
            }

            if (!company.Workers.Any(w => w.Id == worker.Id))
            {
                return false;
            }

            if (worker.StatusId != (int)WorkerStatus.Working)
            {
                return false;
            }

            return true;
        }

        private OrderCompanyDTO GetClosestWorker(Company company, ClientLocationDTO clientInfo)
        {
            double minDistance = -1;
            string minDuration = null;
            int closestWorkerId = -1;


            foreach (var worker in company.Workers)
            {
                if (worker.StatusId != (int)WorkerStatus.Working || worker.CarTypeId != clientInfo.CarType)
                    continue;

                var workerLocation = db.WorkersLastLocation.FindById(worker.Id);

                var distance = Task.Run(async () => await mapService.GetDistanceAsync(clientInfo.Latitude, clientInfo.Longitude, workerLocation.Latitude, workerLocation.Longitude)).Result;
                var duration = Task.Run(async () => await mapService.GetDurationAsync(clientInfo.Latitude, clientInfo.Longitude, workerLocation.Latitude, workerLocation.Longitude)).Result;

                if (minDistance == -1 || distance < minDistance)
                {
                    minDistance = distance;
                    minDuration = duration;
                    closestWorkerId = worker.Id;
                }
            }

            if (minDistance != -1)
            {
                var result = Mapper.Map<Company, OrderCompanyDTO>(company);
                result.Rate = company.CountRate == 0 ? 0 : (double)company.SumRate / (double)company.CountRate;
                result.ClosestDistance = minDistance;
                result.ClosestDuration = minDuration;
                result.ClosestWorkerId = closestWorkerId;

                return result;
            }

            return null;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
