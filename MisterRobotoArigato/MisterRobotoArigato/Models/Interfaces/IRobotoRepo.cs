using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// Interface layer to communicate with the inventory database
    /// </summary>
    public interface IRobotoRepo
    {
        Task<HttpStatusCode> CreateProduct(Product product);

        Task<Product> GetProductById(int? id);

        Task<List<Product>> GetProducts();

        Task<Product> UpdateProduct(int id, Product product);

        Task<HttpStatusCode> DeleteProduct(int id);
    }
}
