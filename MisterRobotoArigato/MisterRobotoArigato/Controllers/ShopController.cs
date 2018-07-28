using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRobotoRepo _robotoRepo;
        private readonly IBasketRepo _basketRepo;
        private readonly IOrderRepo _orderRepo;

        private readonly IConfiguration Configuration;
        private UserManager<ApplicationUser> _userManager;

        public ShopController(IRobotoRepo robotoRepo, IConfiguration configuration, 
            IBasketRepo basketRepo, IOrderRepo orderRepo, UserManager<ApplicationUser> userManager)
        {
            _robotoRepo = robotoRepo;
            _basketRepo = basketRepo;
            _orderRepo = orderRepo;
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

        /// <summary>
        /// Show the details of a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var foundProduct = await _robotoRepo.GetProductById(id);

            if (foundProduct == null) return NotFound();

            return View(foundProduct);
        }

        /// <summary>
        /// Adds a product to a basket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(int? id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Product product = _robotoRepo.GetProductById(id).Result;
            Basket basket = await _basketRepo.GetUserBasketByEmail(user.Email);
            if (basket == null)
            {
                Basket datBasket = new Basket
                {
                    CustomerEmail = user.Email
                };
                await _basketRepo.CreateBasket(datBasket);
            }

            await _basketRepo.AddProductToBasket(user.Email, product);

            return RedirectToAction(nameof(MyBasket));
        }

        /// <summary>
        /// Updates the quantity of the basket
        /// </summary>
        /// <param name="basketItem"></param>
        /// <returns>takes the user back to their mybasket view</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("ID, ProductID, ProductName, CustomerEmail, Quantity, ImgUrl, Description, UnitPrice")]BasketItem basketItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (basketItem.Quantity == 0)
                    {
                        await _basketRepo.DeleteProductFromBasket(basketItem);
                    }
                    else
                    {
                        await _basketRepo.UpdateBasket(User.Identity.Name, basketItem);
                    }
                }
                catch
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(MyBasket));
        }

        /// <summary>
        /// Delete items from a basket
        /// </summary>
        /// <param name="basketItem"></param>
        /// <returns>the user to the mybasket view</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(BasketItem basketItem)
        {
            await _basketRepo.DeleteProductFromBasket(basketItem);
            return RedirectToAction(nameof(MyBasket));
        }

        /// <summary>
        /// takes the user to the view for mybasket
        /// </summary>
        /// <returns>the basket object associated with a user</returns>
        [Authorize]
        public async Task<IActionResult> MyBasket()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Basket datBasket = _basketRepo.GetUserBasketByEmail(user.Email).Result;

            return View(datBasket);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Order order = await _orderRepo.GetOrderByIDAsync(id);

            // Safeguard to prevent user from cancelling another user's order.
            // Not sure how effective this is, though...
            if (order.UserID != user.Id)
                return RedirectToAction("Index", "Orders");

            await _orderRepo.DeleteOrderAsync(id);
            return RedirectToAction("Index", "Orders");
        }
    }
}