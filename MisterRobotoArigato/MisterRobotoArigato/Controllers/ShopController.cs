using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;

namespace MisterRobotoArigato.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRobotoRepo _robotoRepo;
        private readonly IBasketRepo _basketRepo;
        private readonly IConfiguration Configuration;
        private UserManager<ApplicationUser> _userManager;

        public ShopController(IRobotoRepo robotoRepo, IConfiguration configuration, IBasketRepo basketRepo,
            UserManager<ApplicationUser> userManager)
        {
            _robotoRepo = robotoRepo;
            _basketRepo = basketRepo;
            _userManager = userManager;
            Configuration = configuration;
        }

        /// <summary>
        /// populates the list of items to shop and allows users to search for a specific item by name
        /// </summary>
        /// <param name="searchString">the name of the item to search for</param>
        /// <returns>a list of items on search parameters (shows all products if search string is null)</returns>
        public IActionResult Index(string searchString)
        {
            var products = _robotoRepo.GetProducts().Result.AsQueryable();

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

            var foundProduct = await _robotoRepo.GetProductById(id);

            if (foundProduct == null) NotFound();

            return View(foundProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int? id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Product product = _robotoRepo.GetProductById(id).Result;
            await _basketRepo.AddProductToBasket(user.Email, product);

            return RedirectToAction(nameof(MyBasket));

        }

        public async Task<IActionResult> MyBasket()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Basket datBasket = _basketRepo.GetUserBasketByEmail(user.Email).Result;

            return View(datBasket);
        }
    }
}