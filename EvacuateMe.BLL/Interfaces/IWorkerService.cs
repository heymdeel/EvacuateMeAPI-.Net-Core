using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.BLL.DTO;
using EvacuateMe.BLL.DTO.Workers;
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
        Worker SignIn(SmsDTO smsInfo);
        Worker GetByApiKey(string apiKey);
        bool ChangeStatus(Worker worker, int newStatus);
        bool ChangeLocation(Worker worker, LocationDTO newLocation);
        OrderClientDTO CheckForOrders(Worker worker);

        void Dispose();
    }
}
