using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketRepo _repo;
        private readonly IConfiguration Configuration;

        public BasketController(IBasketRepo repo, IConfiguration configuration)
        {
            _repo = repo;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string email, Product product)
        {
            _repo.AddProductToBasket(email, product);
            return RedirectToAction(nameof(Index));
        }
    }
}
