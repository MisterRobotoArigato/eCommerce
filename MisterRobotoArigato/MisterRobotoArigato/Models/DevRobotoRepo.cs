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

        public async Task<Product> GetProductById(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

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
