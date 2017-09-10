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
            var company = await companyService.GetCompanyByLoginAsync(User.Identity.Name);
            if (company == null)
            {
                return NotFound();
            }

            var workers = companyService.GetWorkersAsync(company.Id);

            return View(workers);
        }

        [HttpGet("add")]
        [Authorize(Roles = "company")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost("sign_up"), ValidateAntiForgeryToken]
        [Authorize(Roles = "company")]
        public async Task<IActionResult> Register(WorkerSignUpDTO workerInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var company = await companyService.GetCompanyByLoginAsync(User.Identity.Name);
            if (company == null)
            {
                return NotFound();
            }

            await workerService.SignUpAsync(workerInfo, company.Id);

            return RedirectToAction("Index", "Home");
        }
    }
}