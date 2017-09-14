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
        Task<bool> WorkerExistsAsync(string phone);
        Task<Worker> SignInAsync(SmsDTO smsInfo);
        Task<Worker> GetByApiKeyAsync(string apiKey);
        Task<bool> ChangeStatusAsync(Worker worker, int newStatus);
        Task<bool> ChangeLocationAsync(Worker worker, LocationDTO newLocation);
        Task<Order> CheckForOrdersAsync(Worker worker);
        Task SignUpAsync(WorkerRegisterDTO workerInfo, int companyId);
    }
}
