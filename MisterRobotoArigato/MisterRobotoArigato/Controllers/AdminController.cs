using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Data;

namespace MisterRobotoArigato.Controllers
{
    public class AdminController : Controller
    {
        private readonly RobotoDbContext _context;
        private readonly IConfiguration Configuration;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return View();
        }
    }
}