using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers.Components
{
    public class ShoppingCart : ViewComponent
    {
        private readonly IRobotoRepo _robotoRepo;
        private readonly IBasketRepo _basketRepo;
        private readonly IConfiguration Configuration;
        private UserManager<ApplicationUser> _userManager;

        public ShoppingCart(IRobotoRepo robotoRepo, IConfiguration configuration, IBasketRepo basketRepo,
            UserManager<ApplicationUser> userManager)
        {
            _robotoRepo = robotoRepo;
            _basketRepo = basketRepo;
            _userManager = userManager;
            Configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            Basket datBasket = _basketRepo.GetUserBasketByEmail(user.Email).Result;

            return View(datBasket);
        }
    }
}
