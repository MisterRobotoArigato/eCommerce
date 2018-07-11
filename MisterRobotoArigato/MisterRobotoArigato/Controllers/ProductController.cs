using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Data;
using MisterRobotoArigato.Models;

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
            List<Product> products = _context.Products.ToList();
            ProductListingVM productListVM = new ProductListingVM();
            productListVM.Products = products;
            return View(productListVM);
        }
    }
}