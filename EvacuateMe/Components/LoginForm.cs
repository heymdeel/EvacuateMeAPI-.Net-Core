﻿using EvacuateMe.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.Components
{
    public class LoginForm : ViewComponent
    {
        private readonly IAuthorizationService authorizationService;

        public LoginForm(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public IViewComponentResult Invoke()
        {
            if (User.IsInRole("company"))
            {
                ViewData["name"] = authorizationService.GetCompanyName(User.Identity.Name);
            }
            else
            {
                ViewData["name"] = "администратор";
            }
            return View();
        }
    }
}