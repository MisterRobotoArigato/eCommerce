using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        private UserManager<ApplicationUser> _userManager;

        public DevOrderRepo(RobotoDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets the order by ID and assign it to an order
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the new order object</returns>
        public async Task<Order> GetOrderByIDAsync(int id)
        {
            List<OrderItem> demItems = await _context.OrderItems.Where(i => i.OrderID == id).ToListAsync();
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

        /// <summary>
        /// Takes in an order object and updates that order
        /// </summary>
        /// <param name="order"></param>
        /// <returns>HTTP Status Code</returns>
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
        public async Task<HttpStatusCode> DeleteOrderAsync(int id)
        {

            try
            {
                Order orderToRemove = await _context.Orders.FirstOrDefaultAsync(o => o.ID == id);
                List<OrderItem> orderItemsToRemove = await _context.OrderItems.Where(i => i.OrderID == orderToRemove.ID).ToListAsync();

                _context.RemoveRange(orderItemsToRemove);
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

        /// <summary>
        /// The logic only adds the last n number of orders to a list of lastNOrders
        /// </summary>
        /// <param name="n"></param>
        /// <returns>list of orders that is n from the end of the list</returns>
        public async Task<List<Order>> GetRecentOrdersAsync(int n)
        {
            List<Order> lastNOrders = await _context.Orders.Skip(Math.Max(0, _context.Orders.Count() - n)).ToListAsync();
            return lastNOrders;
        }

        /// <summary>
        /// The logic only adds the last n number of orders to a list of lastNOrders
        /// </summary>
        /// <param name="n"></param>
        /// <param name="userID"></param>
        /// <returns>list of orders that is n from the end of the list and is associated with a user</returns>
        public async Task<List<Order>> GetRecentOrdersAsync(int n, string userID)
        {
            var orderQuery = _context.Orders.Where(o => o.UserID == userID);
            int orderCount = orderQuery.Count();
            List<Order> lastNOrders = await orderQuery.Skip(Math.Max(0, orderCount - n)).ToListAsync();
            return lastNOrders;
        }

        /// <summary>
        /// finds an address associated with a user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>address</returns>
        public async Task<Address> GetAddressByIDAsync(int id)
        {
            Address address = await _context.Addresses.FirstOrDefaultAsync(a => a.ID == id);
            return address;
        }

        /// <summary>
        /// finds all the orderItems associated with a user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a list orderItems associated with the user id</returns>
        public async Task<List<OrderItem>> GetOrderItemsByOrderIDAsync(int id)
        {
            List<OrderItem> orderItems = await _context.OrderItems.Where(i => i.OrderID == id).ToListAsync();
            return orderItems; 
        }
    }
}