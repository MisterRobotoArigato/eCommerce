using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Data;
using MisterRobotoArigato.Models;

namespace MisterRobotoArigato.Controllers
{
    public class AdminController : Controller
    {
        private readonly RobotoDbContext _context;
        private readonly IConfiguration Configuration;

        public AdminController(RobotoDbContext context, IConfiguration configuration)
        {
            _context = context;
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
        public IActionResult GetAllProducts()
        {
            List<Product> products = _context.Products.ToList();
            ProductListingVM productListVM = new ProductListingVM();
            productListVM.Products = products;
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
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetAllProducts));
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
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(GetAllProducts));
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
                return RedirectToAction(nameof(GetAllProducts));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {//prevents a double post in case a product is getting posted at the same time
                    if (!ProductExists(product.ID))
                    {
                        return RedirectToAction(nameof(GetAllProducts));
                    }
                    throw;
                }
                return RedirectToAction(nameof(GetAllProducts));
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
                return RedirectToAction(nameof(GetAllProducts));
            }

            var product = await _context.Products.FirstOrDefaultAsync(
                p => p.ID == id);
            if (product == null)
            {
                return RedirectToAction(nameof(GetAllProducts));
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
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAllProducts));
        }

        /// <summary>
        /// Checks if a product exists.  If it does, return a true value
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.ID == id);
        }
    }
}