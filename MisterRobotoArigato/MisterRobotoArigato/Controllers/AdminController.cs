using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly IRobotoRepo _robotoRepo;
        private readonly IConfiguration Configuration;
        private readonly IOrderRepo _orderRepo;
        //private readonly ICheckoutRepo _checkoutRepo;

        public AdminController(IRobotoRepo robotoRepo, IConfiguration configuration, IOrderRepo orderRepo)
        {
            _robotoRepo = robotoRepo;
            _orderRepo = orderRepo;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets all products and displays them to the view
        /// </summary>
        /// <returns></returns>
        [HttpGet, ActionName("ViewAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await _robotoRepo.GetProducts();
            ProductListingVM productListVM = new ProductListingVM
            {
                Products = products
            };
            return View(productListVM);
        }

        /// <summary>
        /// Displays the view of the create page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Allows user to add a product with the product properties added
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID, Name, SKU, Price, Description, ImgUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _robotoRepo.CreateProduct(product);
                return RedirectToAction("ViewAll");
            }
            return View(product);
        }

        /// <summary>
        /// Finds the product by ID and throws it back to the View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _robotoRepo.GetProductById(id);
            if (product == null)
            {
                return RedirectToAction("ViewAll");
            }
            return View(product);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var foundProduct = await _robotoRepo.GetProductById(id);

            if (foundProduct == null) return NotFound();

            return View(foundProduct);
        }

        /// <summary>
        /// Updates an existing product, otherwise user is redirected back to ViewAllProducts
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>Throws the found product to the view.
        /// Otherwise, redirects the user back to view all products
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID, Name, SKU, Price, Description, ImgUrl")] Product product)
        {
            if (id != product.ID)
            {
                return RedirectToAction("ViewAll");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _robotoRepo.UpdateProduct(id, product);
                }
                catch (DbUpdateConcurrencyException)
                {//prevents a double post in case a product is getting posted at the same time
                    if (!ProductExistsAsync(product.ID).Result)
                    {
                        return RedirectToAction("ViewAll");
                    }
                    throw;
                }
                return RedirectToAction("ViewAll");
            }
            return View(product);
        }

        /// <summary>
        /// Displays the view for deleting an item.  Method finds the item and
        /// passes it to the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user to view all products if the product id doesn't match.
        /// Otherwise, throws the product found to the View
        /// </returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("ViewAll");
            }

            var product = await _robotoRepo.GetProductById(id);
            if (product == null)
            {
                return RedirectToAction("ViewAll");
            }
            return View(product);
        }

        /// <summary>
        /// Deletes a product by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>redirects user to view all products</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmAsync(int id)
        {
            await _robotoRepo.DeleteProduct(id);
            return RedirectToAction("ViewAll");
        }
        
        public async Task<IActionResult> RecentOrders()
        {
            List<Order> recentOrders = await _orderRepo.GetRecentOrdersAsync(20);
            OrderListViewModel datOrderListVM = new OrderListViewModel
            {
                Orders = recentOrders,
            };

            return View(datOrderListVM);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            Order datOrder = await _orderRepo.GetOrderByIDAsync(id);
            datOrder.Address = await _orderRepo.GetAddressByIDAsync(datOrder.AddressID);
            return View(datOrder);
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order datOrder = await _orderRepo.GetOrderByIDAsync(id);
            datOrder.Address = await _orderRepo.GetAddressByIDAsync(datOrder.AddressID);
            return View(datOrder);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderConfirmed(int id)
        {
            await _orderRepo.DeleteOrderAsync(id);
            return RedirectToAction(nameof(RecentOrders));
        }

        /// <summary>
        /// Checks if a product exists.  If it does, return a true value
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private async Task<bool> ProductExistsAsync(int id)
        {
            var products = await _robotoRepo.GetProducts();
            return products.Any(p => p.ID == id);
        }
    }
}