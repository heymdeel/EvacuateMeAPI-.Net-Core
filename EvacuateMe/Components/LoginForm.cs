using EvacuateMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.Components
{
    public class LoginForm : ViewComponent
    {
        private readonly ICompanyService companyService;

        public LoginForm(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public async Task<IViewComponentResult> Invoke()
        {
            if (User.IsInRole("company"))
            {
                ViewData["name"] = await companyService.GetCompanyNameAsync(User.Identity.Name);
            }
            else
            {
                ViewData["name"] = "администратор";
            }
            return View();
        }
    }
}
