using EvacuateMe.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EvacuateMe.BLL.DTO.CompanyDTO;
using EvacuateMe.DAL.Interfaces;
using AutoMapper;
using EvacuateMe.DAL.Entities;
using System.Linq;

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

        public void AddCommpany(CompanyRegisterDTO companyInfo)
        {
            var company = Mapper.Map<CompanyRegisterDTO, Company>(companyInfo);
            company.ApiKey = encryptService.GenerateHash(company.Login, company.ContactPhone);
            db.Companies.Create(company);
            db.Users.Create(new User()
            {
                Login = company.Login,
                Password = encryptService.GeneratePassword(company.Login, company.Password),
                CompanyId = company.Id,
                RoleId = 2
            });
        }

        public IEnumerable<CompanyDTO> GetCompanies()
        {
            List<CompanyDTO> result = new List<CompanyDTO>();
            foreach (var c in db.Companies.Get())
            {
                var company = Mapper.Map<Company, CompanyDTO>(c);
                company.Rate = c.CountRate == 0 ? 0 : (double)c.SumRate / c.CountRate;
                result.Add(company);
            }

            return result;
        }

        public Company GetCompanyByLogin(string login)
        {
            return db.Companies.FirstOrDefault(c => c.Login == login);
        }

        public string GetCompanyName(string login)
        {
            var company = db.Companies.FirstOrDefault(c => c.Login == login);

            return company?.Name;
        }

        public IEnumerable<WorkerDTO> GetWorkers(int companyId)
        {
            var workers = db.Workers.Get(w => w.CompanyId == companyId).ToList();
            var result = new List<WorkerDTO>();
            foreach (var w in workers)
            {
                result.Add(Mapper.Map<Worker, WorkerDTO>(w));
            }

            return result;
        }
    }
}
