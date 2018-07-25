using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Data;

namespace MisterRobotoArigato.Models
{
    public class DevRobotoRepo : IRobotoRepo
    {
        private RobotoDbContext _context;

        public DevRobotoRepo(RobotoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> CreateProduct(Product product)
        {
            try
            {
                var result = await _context.AddAsync(product);
                await _context.SaveChangesAsync();
            }

            catch
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.Created;
        }

        /// <summary>
        /// Delete a product 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }

            return HttpStatusCode.Created;
        }

        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the product by ID</returns>
        public async Task<Product> GetProductById(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>a list of product</returns>
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Updates as product info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>the product by id that has been updated</returns>
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return null;
            }
            var datProduct = await _context.Products.FindAsync(id);
            return datProduct;
        }
    }
}
