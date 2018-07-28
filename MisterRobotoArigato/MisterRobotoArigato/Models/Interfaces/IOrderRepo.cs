using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    /// <summary>
    /// Interface layer to communicate with the Checkout model
    /// </summary>
    public interface IOrderRepo
    {
        Task<HttpStatusCode> CreateAddress(Address address);

        Task<HttpStatusCode> AddOrderAsync(Order Order);

        Task<HttpStatusCode> AddOrderItemToOrderAsync(OrderItem orderItem);

        Task<Order> GetOrderByIDAsync(int id);

        Task<HttpStatusCode> UpdateOrderAsync(Order order);

        Task<HttpStatusCode> DeleteOrderAsync(int id);

        //Task<HttpStatusCode> AdminDeleteOrderAsync(int id);

        Task<List<Order>> GetRecentOrdersAsync(int n);

        Task<List<Order>> GetRecentOrdersAsync(int n, string userID);

        Task<Address> GetAddressByIDAsync(int id);

        Task<List<OrderItem>> GetOrderItemsByOrderIDAsync(int id);
    }
}
