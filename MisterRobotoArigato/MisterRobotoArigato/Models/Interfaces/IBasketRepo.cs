using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public interface IBasketRepo
    {
        Task<HttpStatusCode> AddProductToBasket(string email, Product product);

        Task<Basket> GetUserBasketByEmail(string email);

        Task<Basket> UpdateBasket(string email, Basket basket);

        Task<HttpStatusCode> DeleteProductFromBasket(string email, Basket basket);
    }
}
