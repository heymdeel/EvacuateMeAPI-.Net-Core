using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IClientService
    {
        bool ValidatePhone(string phone);
        Task<bool> ClientExistsAsync(string phone);
        Task<string> SignUpAsync(ClientRegisterDTO client);
        Task<Client> SignInAsync(SmsDTO smsInfo);
        Task<Client> GetByApiKeyAsync(string apiKey);
        Task ChangeCarAsync(Client client, CarDTO newCar);
        Task<IEnumerable<CarType>> GetCarTypesAsync();
    }
}
