using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Data;
using MisterRobotoArigato.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class DevBasketRepo : IBasketRepo
    {
        private RobotoDbContext _context;

        public DevBasketRepo(RobotoDbContext context)
        {
            _context = context;
        }

        public async Task<HttpStatusCode> AddProductToBasket(string email, Product product)
        {
            try
            {
                Basket datBasket = GetUserBasketByEmail(email).Result;
                bool basketHasItem = datBasket.Products.Contains(product);
                if (basketHasItem == true)
                {
                    await UpdateBasket(email, datBasket);
                    return HttpStatusCode.Created;
                }

                BasketDetail datBD = new BasketDetail();
                datBD.ProductID = product.ID;
                datBD.CustomerEmail = email;
                datBD.Quantity = 1;
                datBD.UnitPrice = product.Price;

                await _context.AddAsync(datBD);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }

            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public Task<HttpStatusCode> DeleteProductFromBasket(string email, Basket basket)
        {
            throw new NotImplementedException();
        }

        public async Task<Basket> GetUserBasketByEmail(string email)
        {
            var prodInts = _context.BasketDetails.Where(d => d.CustomerEmail == email).Select(p => p.ProductID);
            List<Product> demProducts = _context.Products.Where(p => prodInts.Contains(p.ID)).ToList();
            Basket datBasket = await _context.Baskets.FirstOrDefaultAsync(b => b.CustomerEmail == email);
            datBasket.Products = demProducts;
            return datBasket;
        }

        public Task<Basket> UpdateBasket(string email, Basket basket)
        {
            throw new NotImplementedException();
        }
    }
}
