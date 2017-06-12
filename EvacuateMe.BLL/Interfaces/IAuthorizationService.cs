using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        User Login(string login, string password);
        string GetCompanyName(string login);
    }
}
