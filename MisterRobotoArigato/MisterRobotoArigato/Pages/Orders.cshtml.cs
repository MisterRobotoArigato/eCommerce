using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [BindProperty, Display(Name = "First Name")]
        public string LastName { get; set; }

        public OrdersModel(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, IOrderRepo orderRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _orderRepo = orderRepo;

        }

        /// <summary>
        /// on page load, the most recent 3 orders, associated with a user, will be fetched
        /// </summary>
        /// <returns>no returns</returns>
        public async Task OnGet()
        {
            //finds the user and the orders associated with them
            var user = await _userManager.GetUserAsync(User);

            FirstName = user.FirstName;
            LastName = user.LastName;

            //fetches the last 3 that matches the user id and stores it
            Orders = await _orderRepo.GetRecentOrdersAsync(3, user.Id);
            foreach (Order order in Orders)
            {
                order.Address = await _orderRepo.GetAddressByIDAsync(order.AddressID);
                order.OrderItems = await _orderRepo.GetOrderItemsByOrderIDAsync(order.ID);
            }
        }
    }
}