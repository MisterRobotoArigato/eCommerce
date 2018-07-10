using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MisterRobotoArigato.Data;

namespace MisterRobotoArigato.Models
{
    public class DevProductRepo : IProductRepo
    {
        private RobotoDbContext _context;

        public DevProductRepo(RobotoDbContext context)
        {
            _context = context;
        }

        public Task<IActionResult> CreateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> UpdateProduct(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}
