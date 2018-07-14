using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;
using Microsoft.AspNetCore.Authorization;

namespace MisterRobotoArigato.Controllers
{
    public class HomeController : Controller
    {
        private IRobotoRepo _repo;

        public HomeController(IRobotoRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy="IsDoge")]
        public IActionResult Doge()
        {
            return View();
        }
    }
}
