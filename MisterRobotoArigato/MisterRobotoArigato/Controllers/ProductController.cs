using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Data;

namespace MisterRobotoArigato.Controllers
{
    public class ProductController : Controller
    {
        private readonly RobotoDbContext _context;
        private readonly IConfiguration Configuration;

        public ProductController(RobotoDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var products = _context.Products;
            return View();
        }
    }
}