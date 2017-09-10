using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EvacuateMe.BLL.Interfaces;
using System.Security.Claims;
using EvacuateMe.DAL.Entities;

namespace EvacuateMe.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HttpGet, Route("Home"), Route("")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: about
        [HttpGet, Route("about")]
        public ActionResult About()
        {
            return View();
        }
    }
}
