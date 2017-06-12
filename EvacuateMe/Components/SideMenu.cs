using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvacuateMe.Components
{
    public class SideMenu : ViewComponent
    {
        Dictionary<string, string> _categories;

        public SideMenu()
        {
            _categories = new Dictionary<string, string>()
            {
                {"Главная", "/" },
                {"Компании", "/companies" },
                {"О нас", "/about" }
            };
        }

        public IViewComponentResult Invoke()
        {
            return View(_categories);
        }
    }
}
