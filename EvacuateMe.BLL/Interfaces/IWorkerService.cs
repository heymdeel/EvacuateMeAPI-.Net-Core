using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IWorkerService
    {
        bool ValidatePhone(string phone);
        bool WorkerExists(string phone);
        Worker SignIn(SmsInfo smsInfo);
        Worker GetByApiKey(string apiKey);
        bool ChangeStatus(Worker worker, int newStatus);
        bool ChangeLocation(Worker worker, Location newLocation);
        IEnumerable<LocationHistoryDTO> GetLocationHistory(Worker worker);
        Task<OrderInfoDTO> CheckForOrdersAsync(Worker worker);

        void Dispose();
    }
}
