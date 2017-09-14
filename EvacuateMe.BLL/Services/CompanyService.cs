using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EvacuateMe.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;
using EvacuateMe.DAL;
using EvacuateMe.BLL.DTO;

namespace EvacuateMe.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork db;
        private readonly IEncrypt encryptService;

        public CompanyService(IUnitOfWork db, IEncrypt encryptService)
        {
            this.db = db;
            this.encryptService = encryptService;
        }

        public async Task AddCommpanyAsync(CompanyRegisterDTO companyInfo)
        {
            Company company = Mapper.Map<CompanyRegisterDTO, Company>(companyInfo);
            company.ApiKey = encryptService.GenerateHash(company.Login, company.ContactPhone);
            await db.Companies.CreateAsync(company);
            await db.Users.CreateAsync(new User()
            {
                Login = company.Login,
                Password = encryptService.GeneratePassword(company.Login, company.Password),
                CompanyId = company.Id,
                RoleId = 2
            });
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await db.Companies.GetAsync();
        }

        public async Task<Company> GetCompanyByLoginAsync(string login)
        {
            return await db.Companies.FirstOrDefaultAsync(c => c.Login == login);
        }

        public async Task<string> GetCompanyNameAsync(string login)
        {
            Company company = await db.Companies.FirstOrDefaultAsync(c => c.Login == login);

            return company?.Name;
        }

        public async Task<IEnumerable<Worker>> GetWorkersAsync(int companyId)
        {
            return await db.Workers.GetAsync(w => w.CompanyId == companyId);
        }
    }
}
