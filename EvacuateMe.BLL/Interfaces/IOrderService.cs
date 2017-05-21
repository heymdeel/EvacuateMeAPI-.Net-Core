using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Orders;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<OrderCompanyDTO> GetListOfCompanies(ClientLocationDTO clientInfo);
        OrderWorkerDTO CreateOrder(Client client, OrderCreateDTO orderInfo);
        LocationDTO GetWorkerLocation(int orderId);
        bool ChangeStatusByClient(int orderId, int newStatus);
        bool ChangeStatusByWorker(int orderId, int newStatus);
        OrderStatus GetOrderStatus(int orderId);
        bool RateOrder(int orderId, int rate);
        CompletedOrderDTO GetOrderInfo(int orderId);
        bool ClientInOrder(int orderId, Client client);
        bool WorkerInOrder(int orderId, Worker worker);
        IEnumerable<OrderHistoryDTO> GetClientHistory(Client client);
        IEnumerable<OrderHistoryDTO> GetWorkerHistory(Worker worker);

        void Dispose();
    }
}
