using EvacuateMe.BLL.BuisnessModels;
using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IClientService
    {
        bool ValidatePhone(string phone);
        bool ClientExists(string phone);
        string SignUp(ClientSignUpDTO client);
        Client SignIn(SmsInfo smsInfo);
        Client GetByApiKey(string apiKey);
        void ChangeCar(Client client, Car newCar);
        
        void Dispose();
    }
}
