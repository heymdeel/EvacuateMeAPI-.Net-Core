using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Interfaces;
using AutoMapper;
using EvacuateMe.DAL.Entities;
using System.Text.RegularExpressions;
using System.Linq;
using EvacuateMe.BLL.BuisnessModels;

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

        public bool ClientExists(string phone)
        {
            var clients = db.Clients.Get(c => c.Phone == phone);
            return (clients.Count() != 0);
        }

        public bool ValidatePhone(string phone)
        {
            return Regex.IsMatch(phone, "^[7-8][0-9]{10}$");
        }

        public string SignUp(ClientSignUpDTO clientData)
        {
            int code = clientData.Code;
            var client = Mapper.Map<ClientSignUpDTO, Client>(clientData);

            var sms = db.SMSCodes.FirstOrDefault(s => s.Phone == client.Phone && s.Code == code);
            if (sms == null)
            {
                return null;
            }

            db.SMSCodes.Remove(sms);
            var apiKey = encrypt.GenerateHash(client.Phone, code.ToString());
            client.ApiKey = apiKey;
            db.Clients.Create(client);

            return apiKey;
        }

        public Client SignIn(SmsInfo smsInfo)
        {
            var sms = db.SMSCodes.FirstOrDefault(s => s.Phone == smsInfo.Phone && s.Code == smsInfo.Code);
            var client = db.Clients.FirstOrDefault(c => c.Phone == smsInfo.Phone);
            if (sms == null || client == null)
            {
                return null;
            }

            db.SMSCodes.Remove(sms);
            client.ApiKey = encrypt.GenerateHash(sms.Phone, sms.Code.ToString());
            db.Clients.Update(client);
            
            return client;
        }

        public Client GetByApiKey(string apiKey)
        {
            return db.Clients.FirstOrDefault(c => c.ApiKey == apiKey);
        }

        public void ChangeCar(Client client, Car newCar)
        {
            client.CarColour = newCar.Colour;
            client.CarModel = newCar.Model;
            db.Clients.Update(client);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
