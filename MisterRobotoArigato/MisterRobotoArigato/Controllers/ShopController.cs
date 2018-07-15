using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// populates the list of items to shop and allows users to search for a specific item by name
        /// </summary>
        /// <param name="searchString">the name of the item to search for</param>
        /// <returns>a list of items on search parameters (shows all products if search string is null)</returns>
        public IActionResult Index(string searchString)
        {
            var products = _repo.GetProducts().Result.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            ProductListingVM productListVM = new ProductListingVM
            {
                Products = products.ToList()
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
    }
}