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
    }
}
