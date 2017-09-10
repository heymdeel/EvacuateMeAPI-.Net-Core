using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.DTO.CompanyDTO;
using EvacuateMe.DAL.Interfaces;
using AutoMapper;
using EvacuateMe.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CompanyDTO>> GetCompaniesAsync()
        {
            List<CompanyDTO> result = new List<CompanyDTO>();
            foreach (var c in await db.Companies.GetAsync())
            {
                var company = Mapper.Map<Company, CompanyDTO>(c);
                company.Rate = c.CountRate == 0 ? 0 : (double)c.SumRate / c.CountRate;
                result.Add(company);
            }

            return result;
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

        public async Task<IEnumerable<WorkerDTO>> GetWorkersAsync(int companyId)
        {
            var workers = await db.Workers.GetAsync(w => w.CompanyId == companyId);
            var result = new List<WorkerDTO>();
            foreach (var w in workers)
            {
                result.Add(Mapper.Map<Worker, WorkerDTO>(w));
            }

            return result;
        }
    }
}
