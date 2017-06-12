using EvacuateMe.BLL.DTO.CompanyDTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EvacuateMe.BLL.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDTO> GetCompanies();
        void AddCommpany(CompanyRegisterDTO company);
        IEnumerable<WorkerDTO> GetWorkers(int companyId);
        string GetCompanyName(string login);
        Company GetCompanyByLogin(string login);
    }
}
