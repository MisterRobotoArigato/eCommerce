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
        private readonly IRobotoRepo _repo;
        private readonly IConfiguration Configuration;

        public AdminController(IRobotoRepo repo, IConfiguration configuration)
        {
            _repo = repo;
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
            List<Product> products = await _repo.GetProducts();
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
                await _repo.CreateProduct(product);
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
            var product = await _repo.GetProductById(id);
            if (product == null)
            {
                return RedirectToAction("ViewAll");
            }
            return View(product);
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
                    await _repo.UpdateProduct(id, product);
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

            var product = await _repo.GetProductById(id);
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
            await _repo.DeleteProduct(id);
            return RedirectToAction("ViewAll");
        }

        /// <summary>
        /// Checks if a product exists.  If it does, return a true value
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private async Task<bool> ProductExistsAsync(int id)
        {
            var products = await _repo.GetProducts();
            return products.Any(p => p.ID == id);
        }
    }
}