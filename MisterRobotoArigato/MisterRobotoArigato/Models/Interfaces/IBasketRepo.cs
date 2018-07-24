using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// Interface layer to communicate with the basket model
    /// </summary>
    public interface IBasketRepo
    {
        Task<HttpStatusCode> CreateBasket(Basket basket);

        Task<HttpStatusCode> AddProductToBasket(string email, Product product);

        Task<Basket> GetUserBasketByEmail(string email);

        Task<HttpStatusCode> UpdateBasket(string email, BasketItem basketItem);

        Task<HttpStatusCode> DeleteProductFromBasket(BasketItem basketItem);

        Task<HttpStatusCode> ClearOutBasket(List<BasketItem> basketItems);
    }
}
