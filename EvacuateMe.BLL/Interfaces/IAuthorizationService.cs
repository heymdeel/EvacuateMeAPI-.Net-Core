using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface IAuthorizationService
    {
        Task<User> LoginAsync(string login, string password);
    }
}
