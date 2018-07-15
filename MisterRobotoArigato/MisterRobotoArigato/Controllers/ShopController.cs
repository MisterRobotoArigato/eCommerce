using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;

namespace MisterRobotoArigato.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRobotoRepo _repo;
        private readonly IConfiguration Configuration;

        public ShopController(IRobotoRepo repo, IConfiguration configuration)
        {
            _repo = repo;
            Configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _repo.GetProducts();
            ProductListingVM productListVM = new ProductListingVM
            {
                Products = products
            };
            return View(productListVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var foundProduct = await _repo.GetProductById(id);

            if (foundProduct == null) NotFound();

            return View(foundProduct);
        }

        //public async Task<IActionResult> Search(string searchString)
        //{
        //    var products = from p in _repo.GetProducts().Result.ToList() select p;

        //    if(!String.IsNullOrEmpty(searchString))
        //    {
        //        products = products.Where(p => p.Name.Contains(searchString));
        //    }
        //    ProductListingVM productListVM = new ProductListingVM
        //    {
        //        Products = (List<Product>)products
        //    };

        //    return View(productListVM);
        //}
    }
}