using EvacuateMe.BLL.Interfaces;
using EvacuateMe.DAL;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<User> LoginAsync(string login, string password)
        {
            string hash = encryptService.GeneratePassword(login, password);

            return await db.Users.FirstOrDefaultAsync(filter: u => u.Login == login && u.Password == hash, include: u => u.Role);
        }
    }
}
