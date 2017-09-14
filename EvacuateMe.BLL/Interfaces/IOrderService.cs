using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderCompanyDTO>> GetListOfCompaniesAsync(ClientLocationDTO clientInfo);
        Task<Order> CreateOrderAsync(Client client, OrderCreateDTO orderInfo);
        Task<LocationDTO> GetWorkerLocationAsync(int orderId);
        Task<bool> ChangeStatusByClientAsync(int orderId, int newStatus);
        Task<bool> ChangeStatusByWorkerAsync(int orderId, int newStatus);
        Task<OrderStatus> GetOrderStatusAsync(int orderId);
        Task<bool> RateOrderAsync(int orderId, int rate);
        Task<Order> GetOrderInfoAsync(int orderId);
        Task<bool> ClientInOrderAsync(int orderId, Client client);
        Task<bool> WorkerInOrderAsync(int orderId, Worker worker);
        Task<IEnumerable<Order>> GetClientHistoryAsync(Client client);
        Task<IEnumerable<Order>> GetWorkerHistoryAsync(Worker worker);
    }
}
