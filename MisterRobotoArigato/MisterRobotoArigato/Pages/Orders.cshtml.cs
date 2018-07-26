using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MisterRobotoArigato.Models;

namespace MisterRobotoArigato.Pages
{
    [Authorize]
    public class OrdersModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IOrderRepo _orderRepo;

        [BindProperty]
        public List<Order> Orders { get; set; }

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        public OrdersModel(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, IOrderRepo orderRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderRepo = orderRepo;

        }

        public async Task OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            FirstName = user.FirstName;
            LastName = user.LastName;

            Orders = await _orderRepo.GetRecentOrdersAsync(3, user.Id);
            foreach (Order order in Orders)
            {
                order.Address = await _orderRepo.GetAddressByIDAsync(order.AddressID);
                order.OrderItems = await _orderRepo.GetOrderItemsByOrderIDAsync(order.ID);
            }
        }
    }
}