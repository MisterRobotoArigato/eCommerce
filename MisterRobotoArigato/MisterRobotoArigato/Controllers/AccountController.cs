using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MisterRobotoArigato.Models;
using MisterRobotoArigato.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisterRobotoArigato.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            var user = new ApplicationUser
            {
                UserName = rvm.Email,
                Email = rvm.Email,
                FirstName = rvm.FirstName,
                LastName = rvm.LastName
            };

            var result = await _userManager.CreateAsync(user, rvm.Password);

            if (result.Succeeded)
            {
            }
            return View();
        }
    }
}
