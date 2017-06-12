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
        private readonly IAuthorizationService loginService;

        public HomeController(IAuthorizationService loginService)
        {
            this.loginService = loginService;
        }

        // GET: Home
        [HttpGet, Route("Home"), Route("")]
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        // GET: About
        [HttpGet, Route("about")]
        public async Task<ActionResult> About()
        {
            loginService.Init();

            return await Task.Run(() =>
            {
                return View();
            });
        }

        // GET: About
        [HttpGet, Route("companies")]
        public async Task<ActionResult> Companies()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost, Route("login")]
        public async Task<ActionResult> Login(string login, string passwd)
        {
            var user = loginService.Login(login, passwd);

            if (user != null)
            {
                await Authenticate(user); // аутентификация

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }
    }
}
