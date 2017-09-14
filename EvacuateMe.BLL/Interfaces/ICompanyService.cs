using EvacuateMe.BLL.DTO;
using EvacuateMe.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EvacuateMe.BLL.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task AddCommpanyAsync(CompanyRegisterDTO company);
        Task<IEnumerable<Worker>> GetWorkersAsync(int companyId);
        Task<string> GetCompanyNameAsync(string login);
        Task<Company> GetCompanyByLoginAsync(string login);
    }
}
