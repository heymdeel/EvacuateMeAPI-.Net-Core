using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.DTO;
using AutoMapper;
using EvacuateMe.DAL.Entities;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using EvacuateMe.DAL;

namespace EvacuateMe.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork db;
        private readonly IEncrypt encrypt;

        public ClientService(IUnitOfWork db, IEncrypt encrypt)
        {
            this.db = db;
            this.encrypt = encrypt;
        }

        public async Task<bool> ClientExistsAsync(string phone)
        {
            Client client = await db.Clients.FirstOrDefaultAsync(filter: c => c.Phone == phone, selector: c => new Client());
            return client != null;
        }

        public bool ValidatePhone(string phone) => Regex.IsMatch(phone, "^[7-8][0-9]{10}$");

        public async Task<string> SignUpAsync(ClientRegisterDTO clientData)
        {
            int code = clientData.Code;
            Client client = Mapper.Map<ClientRegisterDTO, Client>(clientData);

            SMSCode sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == client.Phone && s.Code == code);
            if (sms == null)
            {
                return null;
            }

            await db.SMSCodes.RemoveAsync(sms);
            string apiKey = encrypt.GenerateHash(client.Phone, code.ToString());
            client.ApiKey = apiKey;
            await db.Clients.CreateAsync(client);

            return apiKey;
        }

        public async Task<Client> SignInAsync(SmsDTO smsInfo)
        {
            SMSCode sms = await db.SMSCodes.FirstOrDefaultAsync(s => s.Phone == smsInfo.Phone && s.Code == smsInfo.Code);
            Client client = await db.Clients.FirstOrDefaultAsync(c => c.Phone == smsInfo.Phone);
            if (sms == null || client == null)
            {
                return null;
            }

            await db.SMSCodes.RemoveAsync(sms);
            client.ApiKey = encrypt.GenerateHash(sms.Phone, sms.Code.ToString());
            await db.Clients.UpdateAsync(client);
            
            return client;
        }

        public async Task<Client> GetByApiKeyAsync(string apiKey)
        {
            return await db.Clients.FirstOrDefaultAsync(c => c.ApiKey == apiKey);
        }

        public async Task ChangeCarAsync(Client client, CarDTO newCar)
        {
            client.CarColour = newCar.Colour;
            client.CarModel = newCar.Model;
            await db.Clients.UpdateAsync(client);
        }

        public async Task<IEnumerable<CarType>> GetCarTypesAsync()
        {
            var carTypes = await db.CarTypes.GetAsync();
            
            return carTypes?.ToList().Count == 0 ? null : carTypes; 
        }
    }
}
