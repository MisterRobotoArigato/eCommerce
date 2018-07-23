using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public interface ICheckoutRepo
    {
        Task<HttpStatusCode> CreateAddress(Address address);

        Task<HttpStatusCode> AddOrderAsync(Order Order);

        Task<HttpStatusCode> AddOrderItemToOrderAsync(OrderItem orderItem);

        Task<Order> GetOrderByIDAsync(int id);

        Task<HttpStatusCode> UpdateOrderAsync(Order order);

        Task<HttpStatusCode> DeleteOrderAsync(string id);
    }
}
