using EvacuateMe.BLL.DTO.CompanyDTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDTO>> GetCompaniesAsync();
        Task AddCommpanyAsync(CompanyRegisterDTO company);
        Task<IEnumerable<WorkerDTO>> GetWorkersAsync(int companyId);
        Task<string> GetCompanyNameAsync(string login);
        Task<Company> GetCompanyByLoginAsync(string login);
    }
}
