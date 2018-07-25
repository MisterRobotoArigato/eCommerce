using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class DevOrderRepo : IOrderRepo
    {
        private RobotoDbContext _context;

        public DevOrderRepo(RobotoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the order by ID and assign it to an order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the new order object</returns>
        public async Task<Order> GetOrderByIDAsync(int id)
        {
            List<OrderItem> demItems = _context.OrderItems.Where(i => i.OrderID == id).ToList();
            Order datOrder = await _context.Orders.FirstOrDefaultAsync(o => o.ID == id);

            if (datOrder != null)
                datOrder.OrderItems = demItems;

            return datOrder;
        }

        /// <summary>
        /// Add to an order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> AddOrderAsync(Order order)
        {
            try
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        /// <summary>
        /// Add an OrderItem to an Order
        /// </summary>
        /// <param name="orderItem"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> AddOrderItemToOrderAsync(OrderItem orderItem)
        {
            try
            {
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<HttpStatusCode> UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        /// <summary>
        /// Deletes an Order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> DeleteOrderAsync(string id)
        {
            try
            {
                Order orderToRemove = await _context.Orders.FirstOrDefaultAsync(o => o.UserID == id);
                List<OrderItem> orderItemsToRemove = await _context.OrderItems.Where(i => i.OrderID == orderToRemove.ID).ToListAsync();

                _context.Remove(orderItemsToRemove);
                _context.Remove(orderToRemove);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        /// <summary>
        /// Creates an address
        /// </summary>
        /// <param name="address"></param>
        /// <returns>HTTP Status Code</returns>
        public async Task<HttpStatusCode> CreateAddress(Address address)
        {
            try
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int n)
        {
            List<Order> lastNOrders = await _context.Orders.Skip(Math.Max(0, _context.Orders.Count() - n)).ToListAsync();
            return lastNOrders;
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int n, string userID)
        {
            List<Order> lastNOrders = await _context.Orders.Where(o => o.UserID == userID).Skip(Math.Max(0, _context.Orders.Count() - n)).ToListAsync();
            return lastNOrders;
        }

        public async Task<Address> GetAddressByIDAsync(int id)
        {
            Address address = await _context.Addresses.FirstOrDefaultAsync(a => a.ID == id);
            return address;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIDAsync(int id)
        {
            List<OrderItem> orderItems = await _context.OrderItems.Where(i => i.OrderID == id).ToListAsync();
            return orderItems; 
        }
    }
}