using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EvacuateMe.BLL.DTO.Workers;

namespace EvacuateMe.Controllers
{
    [Route("workers")]
    public class WorkersWebSiteController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly IWorkerService workerService;

        public WorkersWebSiteController(IWorkerService workerService, ICompanyService companyService)
        {
            this.workerService = workerService;
            this.companyService = companyService;
        }

        [Authorize(Roles = "company")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var company = companyService.GetCompanyByLogin(User.Identity.Name);
            if (company == null)
            {
                return NotFound();
            }

            var workers = companyService.GetWorkers(company.Id);

            return View(workers);
        }

        [HttpGet, Route("add")]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, Route("sign_up")]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Register(WorkerSignUpDTO workerInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var company = companyService.GetCompanyByLogin(User.Identity.Name);
            if (company == null)
            {
                return NotFound();
            }

            workerService.SignUp(workerInfo, company.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}