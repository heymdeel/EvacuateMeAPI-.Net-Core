using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL.Entities;
using EvacuateMe.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuateMe.BLL.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUnitOfWork db;
        private readonly IEncrypt encryptService;

        public AuthorizationService(IUnitOfWork db, IEncrypt encryptService)
        {
            this.db = db;
            this.encryptService = encryptService;
        }

        public User Login(string login, string password)
        {
            var hash = encryptService.GeneratePassword(login, password);
            return db.Users.FirstOrDefaultWithInclude(u => u.Login == login && u.Password == hash, u => u.Role);
        }
    }
}
