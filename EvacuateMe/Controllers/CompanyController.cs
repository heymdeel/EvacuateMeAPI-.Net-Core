using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.BLL.Interfaces;

namespace EvacuateMe.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        // GET: companies
        [HttpGet, Route("companies")]
        public async Task<ActionResult> Index()
        {
            var companies = companyService.GetCompanies();

            return await Task.Run(() =>
            {
                return View(companies);
            });
        }
    }
}