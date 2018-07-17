//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using MisterRobotoArigato.Models;
//using MisterRobotoArigato.Models.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity;

//namespace MisterRobotoArigato.Controllers
//{
//    [Authorize]
//    public class BasketController : Controller
//    {
//        private readonly IBasketRepo _repo;
//        private readonly IConfiguration Configuration;
//        private UserManager<ApplicationUser> _userManager;
//        public BasketController(IBasketRepo repo, IConfiguration configuration, UserManager<ApplicationUser> userManager)
//        {
//            _repo = repo;
//            Configuration = configuration;
//            _userManager = userManager;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddAsync(string email, Product product)
//        {
//            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
//            await _repo.AddProductToBasket(email, product);
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
