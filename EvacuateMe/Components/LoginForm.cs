using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.Components
{
    public class LoginForm : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
