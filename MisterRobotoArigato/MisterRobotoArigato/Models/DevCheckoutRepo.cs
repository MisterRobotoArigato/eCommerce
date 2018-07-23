using Microsoft.EntityFrameworkCore;
using MisterRobotoArigato.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Models
{
    public class DevCheckoutRepo : ICheckoutRepo
    {
        private RobotoDbContext _context;

        public DevCheckoutRepo(RobotoDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIDAsync(int id)
        {
            //var prodInts = _context.OrderItems.Where(i => i.OrderID == id).Select(p => p.ProductID);
            List<OrderItem> demItems = _context.OrderItems.Where(i => i.OrderID == id).ToList();
            Order datOrder = await _context.Orders.FirstOrDefaultAsync(o => o.ID == id);

            if (datOrder != null)
                datOrder.OrderItems = demItems;

            return datOrder;
        }

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

        public async Task<HttpStatusCode> DeleteOrderAsync(string id)
        {
            try
            {
                await _context.Orders.FirstOrDefaultAsync(o => o.UserID == id);
                await _context.SaveChangesAsync();
                return HttpStatusCode.Created;
            }

            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }

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
    }
}
